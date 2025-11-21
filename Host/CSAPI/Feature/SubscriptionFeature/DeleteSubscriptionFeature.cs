using BS.Services.SubscriptionService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.SubscriptionFeature
{
    public class DeleteSubscription : ISubscriptionFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapDelete("/subscription/{subscriptionId:int}", Handle)
               .WithSummary("Delete subscription by ID")
               .Produces<bool>()
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

                var exists = await svc.GetSubscriptionById(subscriptionId, ct);
                if (exists == null)
                    return ApiResponseHelper.Convert(false, false, "Subscription not found", 404, null);

                var result = await svc.DeleteSubscription(subscriptionId, ct);
                return ApiResponseHelper.Convert(true, true, "Subscription deleted successfully", 200, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
