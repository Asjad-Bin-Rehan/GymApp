using BS.Services.RewardCatalogService;
using BS.Services.RewardCatalogService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.RewardCatalogFeature
{
    public class UpdateRewardFeature : IRewardCatalogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdateRewardFeature)}", Handle)
            .WithSummary("Update reward details")
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromBody] UpdateRewardDTO request,
            [FromServices] IRewardCatalogService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var success = await svc.UpdateRewardRaw(request, ct);
                if (!success)
                    return ApiResponseHelper.Convert(false, false, "Reward not found or update failed", 404, null);

                return ApiResponseHelper.Convert(true, true, "Reward updated successfully", 200, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
