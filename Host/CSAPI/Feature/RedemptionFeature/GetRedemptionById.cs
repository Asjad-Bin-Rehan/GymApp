using BS.Services.RedemptionService;
using BS.Services.RedemptionService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.RedemptionFeature
{
    public class GetRedemptionById : IRedemptionFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetRedemptionById)}/{{redemption_id:int}}", Handle)
            .WithSummary("Get redemption by ID")
            .Produces<ResponseRedemptionDTO>()
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
                var redemption = await svc.GetRedemptionById(redemption_id, ct);
                if (redemption == null) return ApiResponseHelper.Convert(false, false, "Redemption not found", 404, null);

                return ApiResponseHelper.Convert(true, true, "Success", 200, redemption);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
