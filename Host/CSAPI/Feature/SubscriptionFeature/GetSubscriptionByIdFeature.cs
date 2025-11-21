using BS.Services.SubscriptionService;
using BS.Services.SubscriptionService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.SubscriptionFeature
{
    public class GetSubscriptionById : IFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("/subscription/{subscriptionId:int}", Handle)
               .WithSummary("Get subscription by ID")
               .Produces<ResponseSubscriptionDTO>()
               .Produces(400)
               .Produces(404)
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            [FromRoute] int subscriptionId,
            [FromServices] ISubscriptionService svc,
            [FromServices] ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (subscriptionId <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid subscriptionId", 400, null);

                var sub = await svc.GetSubscriptionById(subscriptionId, ct);
                if (sub == null)
                    return ApiResponseHelper.Convert(false, false, "Subscription not found", 404, null);

                return ApiResponseHelper.Convert(true, true, "Subscription retrieved successfully", 200, sub);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
