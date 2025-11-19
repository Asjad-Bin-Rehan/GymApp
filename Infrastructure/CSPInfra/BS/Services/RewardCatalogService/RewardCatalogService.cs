using BS.Services.RewardCatalogService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BS.Services.RewardCatalogService
{
    public class RewardCatalogService : IRewardCatalogService
    {
        private readonly AppDbContext _dbContext;

        public RewardCatalogService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // =======================================================
        // CREATE NEW REWARD
        // =======================================================
        public async Task<int> AddRewardRaw(AddRewardDTO request, CancellationToken ct)
        {
            var sql = @"
INSERT INTO rewardcatalog (reward_name, description, points_cost, category, active_status, created_at)
VALUES (@reward_name, @description, @points_cost, @category, @active_status, CURRENT_DATE)
RETURNING reward_id;
";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@reward_name", request.reward_name ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@description", request.description ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@points_cost", request.points_cost));
            cmd.Parameters.Add(new NpgsqlParameter("@category", request.category ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@active_status", request.active_status ?? "Y"));

            await _dbContext.Database.OpenConnectionAsync(ct);
            var rewardIdObj = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt32(rewardIdObj);
        }

        // =======================================================
        // GET REWARD BY ID
        // =======================================================
        public async Task<ResponseRewardDTO?> GetRewardByIdRaw(int rewardId, CancellationToken ct)
        {
            var sql = "SELECT reward_id, reward_name, description, points_cost, category, active_status, created_at FROM rewardcatalog WHERE reward_id = @reward_id";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@reward_id", rewardId));

            await _dbContext.Database.OpenConnectionAsync(ct);
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            if (!reader.HasRows) return null;

            await reader.ReadAsync(ct);
            return new ResponseRewardDTO
            {
                reward_id = reader.GetInt32(0),
                reward_name = reader.GetString(1),
                description = reader.IsDBNull(2) ? null : reader.GetString(2),
                points_cost = reader.GetInt32(3),
                category = reader.IsDBNull(4) ? null : reader.GetString(4),
                active_status = reader.GetString(5),
                created_at = reader.GetDateTime(6)
            };
        }

        // =======================================================
        // LIST ALL REWARDS
        // =======================================================
        public async Task<List<ResponseRewardDTO>> ListAllRewardsRaw(CancellationToken ct)
        {
            var list = new List<ResponseRewardDTO>();
            var sql = "SELECT reward_id, reward_name, description, points_cost, category, active_status, created_at FROM rewardcatalog";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            await _dbContext.Database.OpenConnectionAsync(ct);
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
            {
                list.Add(new ResponseRewardDTO
                {
                    reward_id = reader.GetInt32(0),
                    reward_name = reader.GetString(1),
                    description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    points_cost = reader.GetInt32(3),
                    category = reader.IsDBNull(4) ? null : reader.GetString(4),
                    active_status = reader.GetString(5),
                    created_at = reader.GetDateTime(6)
                });
            }

            return list;
        }

        // =======================================================
        // UPDATE REWARD
        // =======================================================
        public async Task<bool> UpdateRewardRaw(UpdateRewardDTO request, CancellationToken ct)
        {
            var sql = @"
UPDATE rewardcatalog
SET reward_name = @reward_name,
    description = @description,
    points_cost = @points_cost,
    category = @category,
    active_status = @active_status
WHERE reward_id = @reward_id;
";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@reward_id", request.reward_id));
            cmd.Parameters.Add(new NpgsqlParameter("@reward_name", request.reward_name ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@description", request.description ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@points_cost", request.points_cost));
            cmd.Parameters.Add(new NpgsqlParameter("@category", request.category ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@active_status", request.active_status ?? "Y"));

            await _dbContext.Database.OpenConnectionAsync(ct);
            var result = await cmd.ExecuteNonQueryAsync(ct);
            return result > 0;
        }

        // =======================================================
        // DELETE REWARD
        // =======================================================
        public async Task<bool> DeleteRewardRaw(int rewardId, CancellationToken ct)
        {
            // Optional: Check if any redemptions exist
            var checkSql = "SELECT COUNT(1) FROM redemptions WHERE reward_id = @reward_id";
            await using var checkCmd = _dbContext.Database.GetDbConnection().CreateCommand();
            checkCmd.CommandText = checkSql;
            checkCmd.Parameters.Add(new NpgsqlParameter("@reward_id", rewardId));
            await _dbContext.Database.OpenConnectionAsync(ct);
            var count = Convert.ToInt32(await checkCmd.ExecuteScalarAsync(ct));
            if (count > 0) throw new Exception("Cannot delete reward with existing redemptions.");

            var sql = "DELETE FROM rewardcatalog WHERE reward_id = @reward_id";
            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@reward_id", rewardId));
            var result = await cmd.ExecuteNonQueryAsync(ct);
            return result > 0;
        }
    }
}
