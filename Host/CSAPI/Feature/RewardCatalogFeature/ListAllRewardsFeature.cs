using BS.Services.RewardCatalogService;
using BS.Services.RewardCatalogService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.RewardCatalogFeature
{
    public class ListAllRewardsFeature : IRewardCatalogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllRewardsFeature)}", Handle)
            .WithSummary("List all rewards in the catalog")
            .Produces<List<ResponseRewardDTO>>()
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromServices] IRewardCatalogService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var rewards = await svc.ListAllRewardsRaw(ct);
                return ApiResponseHelper.Convert(true, true, "Success", 200, rewards);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
