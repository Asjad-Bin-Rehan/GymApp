using BS.Services.SubscriptionService;
using BS.Services.SubscriptionService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.SubscriptionFeature
{
    public class GetSubscriptionByUserId : ISubscriptionFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("/subscription/user/{userId:int}", Handle)
               .WithSummary("Get subscription by user ID with membership plan details")
               .Produces<ResponseSubscriptionWithPlanDTO>()
               .Produces(400)
               .Produces(404)
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            [FromRoute] int userId,
            [FromServices] ISubscriptionService svc,
            [FromServices] ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (userId <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid userId", 400, null);

                var subscription = await svc.GetSubscriptionByUserId(userId, ct);
                if (subscription == null)
                    return ApiResponseHelper.Convert(false, false, "Subscription not found for this user", 404, null);

                return ApiResponseHelper.Convert(true, true, "Subscription retrieved successfully", 200, subscription);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
