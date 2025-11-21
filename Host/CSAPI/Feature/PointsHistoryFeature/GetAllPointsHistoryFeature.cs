using BS.Services.PointsHistoryService;
using CSAPI.Common;
using CustomHTTP;
using Logger;

namespace CSAPI.Feature.PointsHistoryFeature
{
    public class ListAllPointsHistory : IPointsHistoryFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("/ListAllPointsHistory", Handle)
               .WithSummary("List all points history")
               .Produces<List<BS.Services.PointsHistoryService.DTOs.ResponsePointsHistoryDTO>>()
               .Produces(200)
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            IPointsHistoryService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var history = await svc.ListAllPointsHistory(ct);
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
