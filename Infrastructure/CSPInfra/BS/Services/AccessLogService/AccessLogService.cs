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
        public async Task<bool> AddAccessLogRaw(AddAccessLogDTO request, CancellationToken ct)
        {
            var sql = @"
                INSERT INTO public.accesslogs
                    (user_id, gym_id, access_time, points_earned)
                VALUES
                    (@user_id, @gym_id, @access_time, @points_earned);
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@user_id", request.user_id ?? (object)DBNull.Value),
                new NpgsqlParameter("@gym_id", request.gym_id ?? (object)DBNull.Value),
                new NpgsqlParameter("@access_time", request.access_time ?? (object)DBNull.Value),
                new NpgsqlParameter("@points_earned", request.points_earned ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, ct);
            return true;
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
