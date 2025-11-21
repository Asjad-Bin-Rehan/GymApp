using BS.Services.SubscriptionService;
using BS.Services.SubscriptionService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.SubscriptionFeature
{
    public class ListAllSubscriptions : IFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("/subscriptions", Handle)
               .WithSummary("List all subscriptions")
               .Produces<List<ResponseSubscriptionDTO>>()
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            [FromServices] ISubscriptionService svc,
            [FromServices] ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var subs = await svc.ListAllSubscriptions(ct);
                return ApiResponseHelper.Convert(true, true, "Success", 200, subs);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
