using BS.Services.RedemptionService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BS.Services.RedemptionService
{
    public class RedemptionService : IRedemptionService
    {
        private readonly AppDbContext _dbContext;

        public RedemptionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ===============================
        // CREATE REDEMPTION
        // ===============================
        public async Task<int> CreateRedemption(AddRedemptionDTO dto, CancellationToken ct)
        {
            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);
            await using var transaction = await conn.BeginTransactionAsync(ct);

            try
            {
                // 1️⃣ Get reward and check active status
                var rewardCmd = conn.CreateCommand();
                rewardCmd.Transaction = transaction;
                rewardCmd.CommandText = @"
                    SELECT points_cost, active_status
                    FROM rewardcatalog
                    WHERE reward_id = @reward_id
                ";
                rewardCmd.Parameters.Add(new NpgsqlParameter("@reward_id", dto.reward_id));

                await using var rewardReader = await rewardCmd.ExecuteReaderAsync(ct);
                if (!await rewardReader.ReadAsync(ct))
                    throw new Exception("Reward not found.");

                var points_cost = rewardReader.GetInt32(0);
                var active_status = rewardReader.GetString(1);
                await rewardReader.DisposeAsync();

                if (active_status != "Y")
                    throw new Exception("Reward is inactive.");

                // 2️⃣ Check user points
                var userCmd = conn.CreateCommand();
                userCmd.Transaction = transaction;
                userCmd.CommandText = "SELECT total_points FROM users WHERE user_id = @user_id";
                userCmd.Parameters.Add(new NpgsqlParameter("@user_id", dto.user_id));

                var userPointsObj = await userCmd.ExecuteScalarAsync(ct);
                if (userPointsObj == null) throw new Exception("User not found.");
                var userPoints = Convert.ToInt32(userPointsObj);

                if (userPoints < points_cost)
                    throw new Exception("Insufficient points.");

                // 3️⃣ Deduct points
                var updateUserCmd = conn.CreateCommand();
                updateUserCmd.Transaction = transaction;
                updateUserCmd.CommandText = @"
                    UPDATE users
                    SET total_points = total_points - @points
                    WHERE user_id = @user_id
                ";
                updateUserCmd.Parameters.Add(new NpgsqlParameter("@points", points_cost));
                updateUserCmd.Parameters.Add(new NpgsqlParameter("@user_id", dto.user_id));
                await updateUserCmd.ExecuteNonQueryAsync(ct);

                // 4️⃣ Insert redemption
                var insertRedemptionCmd = conn.CreateCommand();
                insertRedemptionCmd.Transaction = transaction;
                insertRedemptionCmd.CommandText = @"
                    INSERT INTO redemptions (user_id, reward_id, points_spent, status)
                    VALUES (@user_id, @reward_id, @points_spent, @status)
                    RETURNING redemption_id
                ";
                insertRedemptionCmd.Parameters.Add(new NpgsqlParameter("@user_id", dto.user_id));
                insertRedemptionCmd.Parameters.Add(new NpgsqlParameter("@reward_id", dto.reward_id));
                insertRedemptionCmd.Parameters.Add(new NpgsqlParameter("@points_spent", points_cost));
                insertRedemptionCmd.Parameters.Add(new NpgsqlParameter("@status", dto.status));

                var redemptionIdObj = await insertRedemptionCmd.ExecuteScalarAsync(ct);
                var redemptionId = Convert.ToInt32(redemptionIdObj);

                // 5️⃣ Insert points history
                var insertPointsCmd = conn.CreateCommand();
                insertPointsCmd.Transaction = transaction;
                insertPointsCmd.CommandText = @"
                    INSERT INTO pointshistory (user_id, points_change, reason)
                    VALUES (@user_id, @points_change, @reason)
                ";
                insertPointsCmd.Parameters.Add(new NpgsqlParameter("@user_id", dto.user_id));
                insertPointsCmd.Parameters.Add(new NpgsqlParameter("@points_change", -points_cost));
                insertPointsCmd.Parameters.Add(new NpgsqlParameter("@reason", "Reward redemption"));

                await insertPointsCmd.ExecuteNonQueryAsync(ct);

                await transaction.CommitAsync(ct);
                return redemptionId;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        // ===============================
        // READ REDemptions
        // ===============================
        public async Task<List<ResponseRedemptionDTO>> ListAllRedemptions(CancellationToken ct)
        {
            var list = new List<ResponseRedemptionDTO>();
            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT redemption_id, user_id, reward_id, status, points_spent, redemption_date
                FROM redemptions
            ";

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new ResponseRedemptionDTO
                {
                    redemption_id = reader.GetInt32(0),
                    user_id = reader.GetInt32(1),
                    reward_id = reader.GetInt32(2),
                    status = reader.GetString(3),
                    points_spent = reader.GetInt32(4),
                    redemption_date = reader.GetDateTime(5)
                });
            }

            return list;
        }

        public async Task<ResponseRedemptionDTO?> GetRedemptionById(int redemption_id, CancellationToken ct)
        {
            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT redemption_id, user_id, reward_id, status, points_spent, redemption_date
                FROM redemptions
                WHERE redemption_id = @redemption_id
            ";
            cmd.Parameters.Add(new NpgsqlParameter("@redemption_id", redemption_id));

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct)) return null;

            return new ResponseRedemptionDTO
            {
                redemption_id = reader.GetInt32(0),
                user_id = reader.GetInt32(1),
                reward_id = reader.GetInt32(2),
                status = reader.GetString(3),
                points_spent = reader.GetInt32(4),
                redemption_date = reader.GetDateTime(5)
            };
        }

        public async Task<List<ResponseRedemptionDTO>> GetRedemptionsByUserId(int user_id, CancellationToken ct)
        {
            var list = new List<ResponseRedemptionDTO>();
            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT redemption_id, user_id, reward_id, status, points_spent, redemption_date
                FROM redemptions
                WHERE user_id = @user_id
            ";
            cmd.Parameters.Add(new NpgsqlParameter("@user_id", user_id));

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new ResponseRedemptionDTO
                {
                    redemption_id = reader.GetInt32(0),
                    user_id = reader.GetInt32(1),
                    reward_id = reader.GetInt32(2),
                    status = reader.GetString(3),
                    points_spent = reader.GetInt32(4),
                    redemption_date = reader.GetDateTime(5)
                });
            }

            return list;
        }

        // ===============================
        // UPDATE REDEMPTION
        // ===============================
        public async Task<bool> UpdateRedemption(UpdateRedemptionDTO dto, CancellationToken ct)
        {
            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);
            await using var transaction = await conn.BeginTransactionAsync(ct);

            try
            {
                // Get current redemption
                var selectCmd = conn.CreateCommand();
                selectCmd.Transaction = transaction;
                selectCmd.CommandText = @"
                    SELECT user_id, points_spent, status
                    FROM redemptions
                    WHERE redemption_id = @redemption_id
                ";
                selectCmd.Parameters.Add(new NpgsqlParameter("@redemption_id", dto.redemption_id));

                await using var reader = await selectCmd.ExecuteReaderAsync(ct);
                if (!await reader.ReadAsync(ct)) throw new Exception("Redemption not found");

                var user_id = reader.GetInt32(0);
                var points_spent = reader.GetInt32(1);
                var current_status = reader.GetString(2);
                await reader.DisposeAsync();

                if (current_status == "Cancelled" || current_status == "Completed")
                    throw new Exception("Cannot update redemption after completion/cancellation");

                // Update status
                var updateCmd = conn.CreateCommand();
                updateCmd.Transaction = transaction;
                updateCmd.CommandText = @"
                    UPDATE redemptions
                    SET status = @status
                    WHERE redemption_id = @redemption_id
                ";
                updateCmd.Parameters.Add(new NpgsqlParameter("@status", dto.status));
                updateCmd.Parameters.Add(new NpgsqlParameter("@redemption_id", dto.redemption_id));
                await updateCmd.ExecuteNonQueryAsync(ct);

                // Refund points if cancelled
                if (dto.status == "Cancelled")
                {
                    var refundCmd = conn.CreateCommand();
                    refundCmd.Transaction = transaction;
                    refundCmd.CommandText = @"
                        UPDATE users
                        SET total_points = total_points + @points
                        WHERE user_id = @user_id
                    ";
                    refundCmd.Parameters.Add(new NpgsqlParameter("@points", points_spent));
                    refundCmd.Parameters.Add(new NpgsqlParameter("@user_id", user_id));
                    await refundCmd.ExecuteNonQueryAsync(ct);

                    var pointsHistoryCmd = conn.CreateCommand();
                    pointsHistoryCmd.Transaction = transaction;
                    pointsHistoryCmd.CommandText = @"
                        INSERT INTO pointshistory (user_id, points_change, reason)
                        VALUES (@user_id, @points_change, @reason)
                    ";
                    pointsHistoryCmd.Parameters.Add(new NpgsqlParameter("@user_id", user_id));
                    pointsHistoryCmd.Parameters.Add(new NpgsqlParameter("@points_change", points_spent));
                    pointsHistoryCmd.Parameters.Add(new NpgsqlParameter("@reason", "Redemption cancelled - points refunded"));
                    await pointsHistoryCmd.ExecuteNonQueryAsync(ct);
                }

                await transaction.CommitAsync(ct);
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        // ===============================
        // DELETE REDEMPTION
        // ===============================
        public async Task<bool> DeleteRedemption(int redemption_id, CancellationToken ct)
        {
            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            // Check status
            var statusCmd = conn.CreateCommand();
            statusCmd.CommandText = @"
                SELECT status
                FROM redemptions
                WHERE redemption_id = @redemption_id
            ";
            statusCmd.Parameters.Add(new NpgsqlParameter("@redemption_id", redemption_id));

            var statusObj = await statusCmd.ExecuteScalarAsync(ct);
            if (statusObj == null) throw new Exception("Redemption not found");

            if (statusObj.ToString() != "Pending")
                throw new Exception("Only pending redemptions can be deleted");

            // Delete
            var deleteCmd = conn.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM redemptions WHERE redemption_id = @redemption_id";
            deleteCmd.Parameters.Add(new NpgsqlParameter("@redemption_id", redemption_id));
            await deleteCmd.ExecuteNonQueryAsync(ct);

            return true;
        }
    }
}
