using BS.Services.RewardCatalogService;
using BS.Services.RewardCatalogService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.RewardCatalogFeature
{
    public class GetRewardByIdFeature : IRewardCatalogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetRewardByIdFeature)}/{{rewardId:int}}", Handle)
            .WithSummary("Get a reward by its ID")
            .Produces<ResponseRewardDTO>()
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromRoute] int rewardId,
            [FromServices] IRewardCatalogService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var reward = await svc.GetRewardByIdRaw(rewardId, ct);
                if (reward == null)
                    return ApiResponseHelper.Convert(false, false, "Reward not found", 404, null);

                return ApiResponseHelper.Convert(true, true, "Success", 200, reward);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
