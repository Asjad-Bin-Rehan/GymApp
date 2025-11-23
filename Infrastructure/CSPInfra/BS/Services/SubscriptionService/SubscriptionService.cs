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





        public class RenewResult
        {
            public bool Success { get; set; }
            public string? ErrorMessage { get; set; }
        }



        public async Task<RenewResult> RenewOrUpgradeAsync(RenewSubscriptionDTO request, CancellationToken ct)
        {
            var conn = _dbContext.Database.GetDbConnection();

            // ✅ Ensure connection is open
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var transaction = await conn.BeginTransactionAsync(ct);

            try
            {
                int subscriptionId = 0;
                int currentPlanId = 0;

                // 1️⃣ Fetch subscription by user_id using the same open connection
                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = transaction;
                    cmd.CommandText = @"
                SELECT subscription_id, plan_id
                FROM subscriptions
                WHERE user_id = @uid
                ORDER BY end_date DESC
                LIMIT 1";
                    cmd.Parameters.Add(new NpgsqlParameter("@uid", request.user_id));

                    await using var reader = await cmd.ExecuteReaderAsync(ct);
                    if (!reader.HasRows)
                        return new RenewResult { Success = false, ErrorMessage = "Subscription not found" };

                    await reader.ReadAsync(ct);
                    subscriptionId = reader.GetInt32(0);
                    currentPlanId = reader.GetInt32(1);
                }

                // 2️⃣ Prevent downgrade
                if (request.plan_id < currentPlanId)
                    return new RenewResult { Success = false, ErrorMessage = "Cannot downgrade subscription plan" };

                // 3️⃣ Get new plan duration
                int months = 0;
                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = transaction;
                    cmd.CommandText = @"
                SELECT duration_months
                FROM membershipplans
                WHERE plan_id = @plan_id";
                    cmd.Parameters.Add(new NpgsqlParameter("@plan_id", request.plan_id));

                    var res = await cmd.ExecuteScalarAsync(ct);
                    if (res == null)
                        return new RenewResult { Success = false, ErrorMessage = "New plan not found" };

                    months = Convert.ToInt32(res);
                }

                // 4️⃣ Update subscription
                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = transaction;
                    cmd.CommandText = @"
                UPDATE subscriptions
                SET plan_id = @plan_id,
                    end_date = CURRENT_DATE + (@months || ' month')::interval
                WHERE subscription_id = @subscription_id";
                    cmd.Parameters.Add(new NpgsqlParameter("@plan_id", request.plan_id));
                    cmd.Parameters.Add(new NpgsqlParameter("@months", months));
                    cmd.Parameters.Add(new NpgsqlParameter("@subscription_id", subscriptionId));

                    var rows = await cmd.ExecuteNonQueryAsync(ct);
                    if (rows == 0)
                        return new RenewResult { Success = false, ErrorMessage = "Failed to update subscription" };
                }

                // 5️⃣ Add points history
                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = transaction;
                    cmd.CommandText = @"
                INSERT INTO pointshistory (user_id, points_change, reason)
                VALUES (@uid, 20, 'Plan Renewed')";
                    cmd.Parameters.Add(new NpgsqlParameter("@uid", request.user_id));

                    await cmd.ExecuteNonQueryAsync(ct);
                }

                await transaction.CommitAsync(ct);
                return new RenewResult { Success = true };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                return new RenewResult { Success = false, ErrorMessage = "Something went wrong: " + ex.Message };
            }
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
