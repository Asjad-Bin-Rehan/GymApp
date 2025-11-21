using BS.Services.PointsHistoryService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BS.Services.PointsHistoryService
{
    public class PointsHistoryService : IPointsHistoryService
    {
        private readonly AppDbContext _dbContext;

        public PointsHistoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddPointsHistory(AddPointsHistoryDTO dto, CancellationToken ct)
        {
            var sql = @"
INSERT INTO pointshistory (user_id, points_change, reason)
VALUES (@user_id, @points_change, @reason)
RETURNING points_id;
";

            var parameters = new[]
            {
                new NpgsqlParameter("@user_id", dto.user_id),
                new NpgsqlParameter("@points_change", dto.points_change),
                new NpgsqlParameter("@reason", dto.reason ?? (object)DBNull.Value)
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, ct);
            return result;
        }

        public async Task<ResponsePointsHistoryDTO?> GetPointsHistoryById(int pointsId, CancellationToken ct)
        {
            var sql = @"SELECT points_id, user_id, points_change, reason, change_date
                        FROM pointshistory
                        WHERE points_id = @points_id";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@points_id", pointsId));

            await _dbContext.Database.OpenConnectionAsync(ct);
            using var reader = await cmd.ExecuteReaderAsync(ct);

            if (!reader.HasRows) return null;
            await reader.ReadAsync(ct);

            return new ResponsePointsHistoryDTO
            {
                points_id = reader.GetInt32(0),
                user_id = reader.GetInt32(1),
                points_change = reader.GetInt32(2),
                reason = reader.IsDBNull(3) ? null : reader.GetString(3),
                change_date = reader.GetDateTime(4)
            };
        }

        public async Task<List<ResponsePointsHistoryDTO>> ListAllPointsHistory(CancellationToken ct)
        {
            var list = new List<ResponsePointsHistoryDTO>();
            var sql = "SELECT points_id, user_id, points_change, reason, change_date FROM pointshistory";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            await _dbContext.Database.OpenConnectionAsync(ct);

            using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new ResponsePointsHistoryDTO
                {
                    points_id = reader.GetInt32(0),
                    user_id = reader.GetInt32(1),
                    points_change = reader.GetInt32(2),
                    reason = reader.IsDBNull(3) ? null : reader.GetString(3),
                    change_date = reader.GetDateTime(4)
                });
            }

            return list;
        }

        public async Task<List<ResponsePointsHistoryDTO>> GetPointsHistoryByUserId(int userId, CancellationToken ct)
        {
            var list = new List<ResponsePointsHistoryDTO>();
            var sql = @"SELECT points_id, user_id, points_change, reason, change_date
                        FROM pointshistory
                        WHERE user_id = @user_id";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@user_id", userId));

            await _dbContext.Database.OpenConnectionAsync(ct);
            using var reader = await cmd.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
            {
                list.Add(new ResponsePointsHistoryDTO
                {
                    points_id = reader.GetInt32(0),
                    user_id = reader.GetInt32(1),
                    points_change = reader.GetInt32(2),
                    reason = reader.IsDBNull(3) ? null : reader.GetString(3),
                    change_date = reader.GetDateTime(4)
                });
            }

            return list;
        }

        public async Task<bool> UpdatePointsHistory(UpdatePointsHistoryDTO dto, CancellationToken ct)
        {
            var sql = @"
UPDATE pointshistory
SET points_change = COALESCE(@points_change, points_change),
    reason = COALESCE(@reason, reason)
WHERE points_id = @points_id";

            var parameters = new[]
            {
                new NpgsqlParameter("@points_id", dto.points_id),
                new NpgsqlParameter("@points_change", dto.points_change ?? (object)DBNull.Value),
                new NpgsqlParameter("@reason", dto.reason ?? (object)DBNull.Value)
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, ct);
            return result > 0;
        }

        public async Task<bool> DeletePointsHistory(int pointsId, CancellationToken ct)
        {
            var sql = "DELETE FROM pointshistory WHERE points_id = @points_id";
            var result = await _dbContext.Database.ExecuteSqlRawAsync(sql, new NpgsqlParameter("@points_id", pointsId), ct);
            return result > 0;
        }
    }
}
