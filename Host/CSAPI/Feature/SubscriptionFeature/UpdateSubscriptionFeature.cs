using BS.Services.SubscriptionService;
using BS.Services.SubscriptionService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.SubscriptionFeature
{
    public class UpdateSubscription : IFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapPut("/update-subscription", Handle)
               .WithSummary("Update subscription details")
               .Produces<bool>()
               .Produces(400)
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            [FromBody] UpdateSubscriptionDTO request,
            [FromServices] ISubscriptionService svc,
            [FromServices] ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (request.subscription_id <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid subscription_id", 400, null);

                var result = await svc.UpdateSubscription(request, ct);
                return ApiResponseHelper.Convert(true, true, "Subscription updated successfully", 200, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
