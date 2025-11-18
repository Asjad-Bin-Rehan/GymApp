using BS.Services.MembershipPlanService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BS.Services.MembershipPlanService
{
    public class MembershipPlanService : IMembershipPlanService
    {
        private readonly AppDbContext _dbContext;

        public MembershipPlanService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddMembershipPlanRaw(AddMembershipPlanDTO request, CancellationToken ct)
        {
            var sql = @"
INSERT INTO MembershipPlans (plan_name, duration_months, price, description)
VALUES (@plan_name, @duration_months, @price, @description);
";
            var parameters = new[]
            {
                new NpgsqlParameter("@plan_name", request.plan_name ?? (object)DBNull.Value),
                new NpgsqlParameter("@duration_months", request.duration_months),
                new NpgsqlParameter("@price", request.price),
                new NpgsqlParameter("@description", request.description ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, ct);
            return true;
        }

        public async Task<ResponseMembershipPlanDTO?> GetMembershipPlanByIdRaw(int planId, CancellationToken ct)
        {
            var sql = @"
SELECT plan_id, plan_name, duration_months, price, description
FROM MembershipPlans
WHERE plan_id = @plan_id
";
            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@plan_id", planId));

            await _dbContext.Database.OpenConnectionAsync(ct);

            using var reader = await cmd.ExecuteReaderAsync(ct);
            if (!reader.HasRows) return null;

            await reader.ReadAsync(ct);
            return new ResponseMembershipPlanDTO
            {
                plan_id = reader.GetInt32(0),
                plan_name = reader.GetString(1),
                duration_months = reader.GetInt32(2),
                price = reader.GetDecimal(3),
                description = reader.GetString(4)
            };
        }

        public async Task<List<ResponseMembershipPlanDTO>> ListAllMembershipPlansRaw(CancellationToken ct)
        {
            var list = new List<ResponseMembershipPlanDTO>();
            var sql = "SELECT plan_id, plan_name, duration_months, price, description FROM MembershipPlans";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            await _dbContext.Database.OpenConnectionAsync(ct);

            using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new ResponseMembershipPlanDTO
                {
                    plan_id = reader.GetInt32(0),
                    plan_name = reader.GetString(1),
                    duration_months = reader.GetInt32(2),
                    price = reader.GetDecimal(3),
                    description = reader.GetString(4)
                });
            }

            return list;
        }

        public async Task<bool> UpdateMembershipPlanRaw(UpdateMembershipPlanDTO request, CancellationToken ct)
        {
            var sql = @"
UPDATE MembershipPlans
SET plan_name = @plan_name,
    duration_months = @duration_months,
    price = @price,
    description = @description
WHERE plan_id = @plan_id
";
            var parameters = new[]
            {
                new NpgsqlParameter("@plan_name", request.plan_name ?? (object)DBNull.Value),
                new NpgsqlParameter("@duration_months", request.duration_months),
                new NpgsqlParameter("@price", request.price),
                new NpgsqlParameter("@description", request.description ?? (object)DBNull.Value),
                new NpgsqlParameter("@plan_id", request.plan_id)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, ct);
            return true;
        }

        public async Task<bool> DeleteMembershipPlanRaw(int planId, CancellationToken ct)
        {
            var sql = "DELETE FROM MembershipPlans WHERE plan_id = @plan_id";
            await _dbContext.Database.ExecuteSqlRawAsync(sql, new NpgsqlParameter("@plan_id", planId), ct);
            return true;
        }
    }
}
