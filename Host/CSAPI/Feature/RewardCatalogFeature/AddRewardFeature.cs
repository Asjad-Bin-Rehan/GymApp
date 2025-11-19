using BS.Services.RewardCatalogService;
using BS.Services.RewardCatalogService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.RewardCatalogFeature
{
    public class AddRewardFeature : IRewardCatalogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddRewardFeature)}", Handle)
            .WithSummary("Add a new reward to the catalog")
            .Produces(200)
            .Produces(400)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromBody] AddRewardDTO request,
            [FromServices] IRewardCatalogService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.reward_name) || request.points_cost <= 0)
                {
                    return ApiResponseHelper.Convert(false, false, "Reward name and positive points cost are required", 400, null);
                }

                var rewardId = await svc.AddRewardRaw(request, ct);

                return ApiResponseHelper.Convert(true, true, "Reward added successfully", 200, new { reward_id = rewardId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
