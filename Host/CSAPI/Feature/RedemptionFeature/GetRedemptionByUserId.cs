using BS.Services.RedemptionService;
using BS.Services.RedemptionService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.RedemptionFeature
{
    public class GetRedemptionsByUserId : IRedemptionFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetRedemptionsByUserId)}/{{user_id:int}}", Handle)
            .WithSummary("Get all redemptions for a user")
            .Produces<List<ResponseRedemptionDTO>>()
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromRoute] int user_id,
            [FromServices] IRedemptionService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var redemptions = await svc.GetRedemptionsByUserId(user_id, ct);
                if (!redemptions.Any()) return ApiResponseHelper.Convert(false, false, "No redemptions found", 404, null);

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
