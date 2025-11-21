using BS.Services.SubscriptionService;
using BS.Services.SubscriptionService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.SubscriptionFeature
{
    public class AddSubscription : IFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("/add-subscription", Handle)
               .WithSummary("Add a new subscription")
               .Produces(200)
               .Produces(400)
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            [FromBody] AddSubscriptionDTO request,
            [FromServices] ISubscriptionService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (request.user_id <= 0 || request.plan_id <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid user_id or plan_id", 400, null);

                var result = await svc.AddSubscription(request, ct);

                return ApiResponseHelper.Convert(true, true, "Subscription added successfully", 200, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
