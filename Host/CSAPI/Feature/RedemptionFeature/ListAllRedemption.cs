using BS.Services.RedemptionService;
using BS.Services.RedemptionService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.RedemptionFeature
{
    public class ListAllRedemptions : IRedemptionFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllRedemptions)}", Handle)
            .WithSummary("List all redemptions")
            .Produces<List<ResponseRedemptionDTO>>()
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromServices] IRedemptionService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var redemptions = await svc.ListAllRedemptions(ct);
                return ApiResponseHelper.Convert(true, true, "Success", 200, redemptions);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
