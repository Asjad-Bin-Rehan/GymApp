using BS.Services.SubscriptionService;
using BS.Services.SubscriptionService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using DA.AppDbContexts;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CSAPI.Feature.SubscriptionFeature
{
    public class RenewSubscription : IFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("/renew-subscription", Handle)
               .WithSummary("Renew or upgrade a subscription plan")
               .Produces<bool>()
               .Produces(400)
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            [FromBody] RenewSubscriptionDTO request,
            [FromServices] ISubscriptionService svc,
            [FromServices] ICustomLogger logger,
            [FromServices] AppDbContext dbContext,
            CancellationToken ct)
        {
            try
            {
                // Validate input
                if (request.user_id <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid user_id", 400, null);

                if (request.plan_id <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid plan_id", 400, null);

                int subscriptionId;
                int currentPlanId;

                // ✅ Open connection safely
                var conn = dbContext.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open)
                    await conn.OpenAsync(ct);

                await using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT subscription_id, plan_id
                        FROM subscriptions
                        WHERE user_id = @uid
                        ORDER BY end_date DESC
                        LIMIT 1";
                    cmd.Parameters.Add(new NpgsqlParameter("@uid", request.user_id));

                    await using var reader = await cmd.ExecuteReaderAsync(ct);

                    if (!reader.HasRows)
                        return ApiResponseHelper.Convert(false, false, "Active subscription not found for user", 400, null);

                    await reader.ReadAsync(ct);
                    subscriptionId = reader.GetInt32(0);
                    currentPlanId = reader.GetInt32(1);
                }

                if (request.plan_id < currentPlanId)
                    return ApiResponseHelper.Convert(false, false, "Cannot downgrade subscription plan", 400, null);

                var result = await svc.RenewOrUpgradeAsync(
                    new RenewSubscriptionDTO
                    {
                        user_id = request.user_id,
                        plan_id = request.plan_id,
                        SubscriptionId = subscriptionId
                    },
                    ct
                );

                return result.Success
                    ? ApiResponseHelper.Convert(true, true, result.ErrorMessage ?? "Subscription renewed/upgraded successfully", 200, true)
                    : ApiResponseHelper.Convert(false, false, result.ErrorMessage ?? "Failed to renew/upgrade subscription", 400, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
