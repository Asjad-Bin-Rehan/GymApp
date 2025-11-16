using BS.Services.LocationService;
using CSAPI.Common;
using CustomHTTP;
using Logger;

namespace CSAPI.Feature.LocationFeature
{
    public class DeleteLocation : ILocationFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapDelete($"/{nameof(DeleteLocation)}/{{locationId}}", Handle)
            .WithSummary("Delete a location by ID");

        private static async Task<IResult> Handle(int locationId, ILocationService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var success = await svc.DeleteLocationRaw(locationId, ct);
                return ApiResponseHelper.Convert(true, success, "Deleted", 200, success);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ApiResponseHelper.Convert(false, false, "Error", 500, null);
            }
        }
    }
}
