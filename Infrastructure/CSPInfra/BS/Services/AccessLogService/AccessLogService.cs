using BS.Services.AccessLogService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BS.Services.AccessLogService
{
    public class AccessLogService : IAccessLogService
    {
        private readonly AppDbContext _dbContext;

        public AccessLogService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ============================================================
        // ADD ACCESS LOG (RAW SQL)
        // ============================================================
        public async Task<AccessLogResultDTO> AddAccessLogRaw(AddAccessLogDTO request, CancellationToken ct)
        {
            var connection = _dbContext.Database.GetDbConnection();
            await _dbContext.Database.OpenConnectionAsync(ct);

            double gymLat = 0, gymLon = 0;

            // 1. Fetch gym location
            var gymQuery = @"
        SELECT L.latitude, L.longitude
        FROM PartnerGyms PG
        JOIN Locations L ON PG.location_id = L.location_id
        WHERE PG.gym_id = @gym_id
    ";

            await using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = gymQuery;
                cmd.Parameters.Add(new NpgsqlParameter("@gym_id", request.gym_id));

                using var reader = await cmd.ExecuteReaderAsync(ct);

                if (!reader.HasRows)
                    return new AccessLogResultDTO { Success = false, PointsEarned = 0, Message = "Gym not found" };

                await reader.ReadAsync(ct);
                gymLat = reader.GetDouble(0);
                gymLon = reader.GetDouble(1);
            }

            // 2. Validate distance (within 200 meters)
            double distance = CalculateDistance(request.device_lat, request.device_lon, gymLat, gymLon);
            if (distance > 0.3)
                return new AccessLogResultDTO { Success = false, PointsEarned = 0, Message = "User is not near the gym. Please verify gym ID." };

            // 3. Check if user already checked in today
            var checkTodayQuery = @"
        SELECT COUNT(*) 
        FROM AccessLogs 
        WHERE user_id = @user_id AND gym_id = @gym_id AND DATE(access_time) = CURRENT_DATE
    ";
            int checkInCount = 0;
            await using (var cmd2 = connection.CreateCommand())
            {
                cmd2.CommandText = checkTodayQuery;
                cmd2.Parameters.Add(new NpgsqlParameter("@user_id", request.user_id));
                cmd2.Parameters.Add(new NpgsqlParameter("@gym_id", request.gym_id));

                checkInCount = Convert.ToInt32(await cmd2.ExecuteScalarAsync(ct));
            }

            bool alreadyCheckedInToday = checkInCount > 0;

            // 4. Get user's membership plan
            int planId = 1;
            var planQuery = @"
        SELECT plan_id 
        FROM Subscriptions 
        WHERE user_id = @user_id
        ORDER BY subscription_id DESC LIMIT 1
    ";

            await using (var cmd3 = connection.CreateCommand())
            {
                cmd3.CommandText = planQuery;
                cmd3.Parameters.Add(new NpgsqlParameter("@user_id", request.user_id));

                using var reader = await cmd3.ExecuteReaderAsync(ct);
                if (reader.HasRows)
                {
                    await reader.ReadAsync(ct);
                    planId = reader.GetInt32(0);
                }
            }

            // 5. Determine points to award
            int pointsEarned = alreadyCheckedInToday ? 0 : planId switch
            {
                1 => 10,
                2 => 20,
                3 => 30,
                _ => 10
            };

            // 6. Insert into AccessLogs
            var insertLog = @"
        INSERT INTO AccessLogs (user_id, gym_id, points_earned)
        VALUES (@user_id, @gym_id, @points)
    ";
            await _dbContext.Database.ExecuteSqlRawAsync(insertLog, new[]
            {
        new NpgsqlParameter("@user_id", request.user_id),
        new NpgsqlParameter("@gym_id", request.gym_id),
        new NpgsqlParameter("@points", pointsEarned)
    }, ct);

            // 7. Update user total points if > 0
            if (pointsEarned > 0)
            {
                var updatePoints = @"
            UPDATE Users 
            SET total_points = total_points + @points
            WHERE user_id = @user_id
        ";
                await _dbContext.Database.ExecuteSqlRawAsync(updatePoints, new[]
                {
            new NpgsqlParameter("@points", pointsEarned),
            new NpgsqlParameter("@user_id", request.user_id)
        }, ct);

                // 8. Insert PointsHistory
                var insertHistory = @"
            INSERT INTO PointsHistory (user_id, points_change, reason)
            VALUES (@user_id, @points, @reason)
        ";
                string reasonMessage = $"Gym check-in at gym {request.gym_id}";
                await _dbContext.Database.ExecuteSqlRawAsync(insertHistory, new[]
                {
            new NpgsqlParameter("@user_id", request.user_id),
            new NpgsqlParameter("@points", pointsEarned),
            new NpgsqlParameter("@reason", reasonMessage)
        }, ct);
            }

            string message = alreadyCheckedInToday ? "Already checked in today. 0 points awarded." : $"Check-in successful. {pointsEarned} points awarded.";

            return new AccessLogResultDTO
            {
                Success = true,
                PointsEarned = pointsEarned,
                Message = message
            };
        }


        // Haversine formula (distance in KM)
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Radius of Earth in km

            double dLat = (lat2 - lat1) * (Math.PI / 180);
            double dLon = (lon2 - lon1) * (Math.PI / 180);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }


        // ============================================================
        // GET ACCESS LOG BY ID
        // ============================================================
        public async Task<ResponseAccessLogDTO?> GetAccessLogByIdRaw(int logId, CancellationToken ct)
        {
            var sql = @"
                SELECT log_id, user_id, gym_id, access_time, points_earned
                FROM public.accesslogs
                WHERE log_id = @log_id
            ";

            var param = new NpgsqlParameter("@log_id", logId);

            return await _dbContext
                .Database
                .SqlQueryRaw<ResponseAccessLogDTO>(sql, param)
                .FirstOrDefaultAsync(ct);
        }

        // GET ACCESS LOG BY User ID
        public async Task<List<ResponseAccessLogDTO>> GetAccessLogsByUserIdRaw(int userId, CancellationToken ct)
        {
            var sql = @"
        SELECT log_id, user_id, gym_id, access_time, points_earned
        FROM public.accesslogs
        WHERE user_id = @user_id
        ORDER BY access_time DESC
    ";

            var param = new NpgsqlParameter("@user_id", userId);

            return await _dbContext.Database
                .SqlQueryRaw<ResponseAccessLogDTO>(sql, param)
                .ToListAsync(ct);
        }




        // ============================================================
        // LIST ACCESS LOGS (PAGINATED)
        // ============================================================
        public async Task<List<ResponseAccessLogDTO>> ListAllAccessLogsRaw(int limit, int offset, CancellationToken ct)
        {
            var sql = @"
                SELECT log_id, user_id, gym_id, access_time, points_earned
                FROM public.accesslogs
                ORDER BY access_time DESC
                LIMIT @limit OFFSET @offset
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@limit", limit),
                new NpgsqlParameter("@offset", offset)
            };

            return await _dbContext
                .Database
                .SqlQueryRaw<ResponseAccessLogDTO>(sql, parameters)
                .ToListAsync(ct);
        }

        // ============================================================
        // DELETE ACCESS LOG
        // ============================================================
        public async Task<bool> DeleteAccessLogRaw(int logId, CancellationToken ct)
        {
            var sql = @"
        DELETE FROM public.accesslogs
        WHERE log_id = @log_id
    ";

            var param = new NpgsqlParameter("@log_id", logId);

            await _dbContext.Database.ExecuteSqlRawAsync(
                sql,
                new[] { param },   // ✔ parameters must be inside array
                ct                  // ✔ CT is last argument
            );

            return true;
        }

    }
}
