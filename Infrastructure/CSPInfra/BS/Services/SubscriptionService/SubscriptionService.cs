using BS.Services.SubscriptionService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BS.Services.SubscriptionService
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppDbContext _dbContext;

        public SubscriptionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ------------------- CREATE -------------------
        public async Task<int> AddSubscription(AddSubscriptionDTO dto, CancellationToken ct)
        {
            var sql = @"
INSERT INTO subscriptions (user_id, plan_id, start_date, end_date, payment_status)
VALUES (@user_id, @plan_id, @start_date, @end_date, @payment_status)
RETURNING subscription_id;
";

            var parameters = new[]
            {
                new NpgsqlParameter("@user_id", dto.user_id),
                new NpgsqlParameter("@plan_id", dto.plan_id),
                new NpgsqlParameter("@start_date", dto.start_date ?? (object)DBNull.Value),
                new NpgsqlParameter("@end_date", dto.end_date ?? (object)DBNull.Value),
                new NpgsqlParameter("@payment_status", dto.payment_status ?? "Paid")
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, ct);
            return result > 0 ? 1 : 0; // or return subscription_id if needed
        }

        // ------------------- READ -------------------
        public async Task<ResponseSubscriptionDTO?> GetSubscriptionById(int subscriptionId, CancellationToken ct)
        {
            var sql = @"
SELECT subscription_id, user_id, plan_id, start_date, end_date, payment_status
FROM subscriptions
WHERE subscription_id = @subscription_id;
";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@subscription_id", subscriptionId));

            await _dbContext.Database.OpenConnectionAsync(ct);
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            if (!reader.HasRows) return null;
            await reader.ReadAsync(ct);

            return new ResponseSubscriptionDTO
            {
                subscription_id = reader.GetInt32(0),
                user_id = reader.GetInt32(1),
                plan_id = reader.GetInt32(2),
                start_date = reader.GetDateTime(3),
                end_date = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                payment_status = reader.GetString(5)
            };
        }

        public async Task<List<ResponseSubscriptionDTO>> ListAllSubscriptions(CancellationToken ct)
        {
            var list = new List<ResponseSubscriptionDTO>();
            var sql = @"
SELECT subscription_id, user_id, plan_id, start_date, end_date, payment_status
FROM subscriptions;
";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;

            await _dbContext.Database.OpenConnectionAsync(ct);
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
            {
                list.Add(new ResponseSubscriptionDTO
                {
                    subscription_id = reader.GetInt32(0),
                    user_id = reader.GetInt32(1),
                    plan_id = reader.GetInt32(2),
                    start_date = reader.GetDateTime(3),
                    end_date = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                    payment_status = reader.GetString(5)
                });
            }

            return list;
        }

        // ------------------- UPDATE -------------------
        public async Task<bool> UpdateSubscription(UpdateSubscriptionDTO dto, CancellationToken ct)
        {
            var sql = @"
UPDATE subscriptions
SET user_id = @user_id,
    plan_id = @plan_id,
    start_date = @start_date,
    end_date = @end_date,
    payment_status = @payment_status
WHERE subscription_id = @subscription_id;
";

            var parameters = new[]
            {
                new NpgsqlParameter("@subscription_id", dto.subscription_id),
                new NpgsqlParameter("@user_id", dto.user_id ?? (object)DBNull.Value),
                new NpgsqlParameter("@plan_id", dto.plan_id ?? (object)DBNull.Value),
                new NpgsqlParameter("@start_date", dto.start_date ?? (object)DBNull.Value),
                new NpgsqlParameter("@end_date", dto.end_date ?? (object)DBNull.Value),
                new NpgsqlParameter("@payment_status", dto.payment_status ?? (object)DBNull.Value)
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, ct);
            return result > 0;
        }

        // ------------------- DELETE -------------------
        public async Task<bool> DeleteSubscription(int subscriptionId, CancellationToken ct)
        {
            var sql = "DELETE FROM subscriptions WHERE subscription_id = @subscription_id";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@subscription_id", subscriptionId));

            await _dbContext.Database.OpenConnectionAsync(ct);
            var result = await cmd.ExecuteNonQueryAsync(ct);
            return result > 0;
        }
    }
}
