using BS.Services.RedemptionService;
using BS.Services.RedemptionService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.RedemptionFeature
{
    public class UpdateRedemption : IRedemptionFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdateRedemption)}", Handle)
            .WithSummary("Update redemption status")
            .Produces(200)
            .Produces(400)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromBody] UpdateRedemptionDTO request,
            [FromServices] IRedemptionService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var success = await svc.UpdateRedemption(request, ct);
                if (!success) return ApiResponseHelper.Convert(false, false, "Failed to update redemption", 400, null);

                return ApiResponseHelper.Convert(true, true, "Redemption updated", 200, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, ex.Message, 500, null);
            }
        }
    }
}
