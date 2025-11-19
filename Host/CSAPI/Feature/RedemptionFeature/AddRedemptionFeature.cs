using BS.Services.RedemptionService;
using BS.Services.RedemptionService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.RedemptionFeature
{
    public class AddRedemption : IRedemptionFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddRedemption)}", Handle)
            .WithSummary("Create a new redemption")
            .Produces(200)
            .Produces(400)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromBody] AddRedemptionDTO request,
            [FromServices] IRedemptionService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (request.user_id <= 0 || request.reward_id <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid user or reward ID", 400, null);

                var redemptionId = await svc.CreateRedemption(request, ct);
                return ApiResponseHelper.Convert(true, true, "Redemption created", 200, new { redemption_id = redemptionId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, ex.Message, 500, null);
            }
        }
    }
}
