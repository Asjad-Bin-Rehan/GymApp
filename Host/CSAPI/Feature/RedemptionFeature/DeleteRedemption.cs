using BS.Services.RedemptionService;
using BS.Services.RedemptionService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.RedemptionFeature
{
    public class DeleteRedemption : IRedemptionFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapDelete($"/{nameof(DeleteRedemption)}/{{redemption_id:int}}", Handle)
            .WithSummary("Delete a pending redemption")
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromRoute] int redemption_id,
            [FromServices] IRedemptionService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var success = await svc.DeleteRedemption(redemption_id, ct);
                if (!success) return ApiResponseHelper.Convert(false, false, "Cannot delete redemption", 400, null);

                return ApiResponseHelper.Convert(true, true, "Redemption deleted", 200, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, ex.Message, 500, null);
            }
        }
    }
}
