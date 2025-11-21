using BS.Services.PointsHistoryService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PointsHistoryFeature
{
    public class GetPointsHistoryByUserId : IPointsHistoryFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet($"/GetPointsHistoryByUserId/{{userId}}", Handle)
               .WithSummary("Get points history by user ID")
               .Produces<List<BS.Services.PointsHistoryService.DTOs.ResponsePointsHistoryDTO>>()
               .Produces(200)
               .Produces(400)
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            [FromRoute] int userId,
            IPointsHistoryService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (userId <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid user ID", 400, null);

                var history = await svc.GetPointsHistoryByUserId(userId, ct);
                return ApiResponseHelper.Convert(true, true, "Success", 200, history);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
