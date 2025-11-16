using BS.Services.LocationService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.LocationFeature
{
    public class GetLocationById : ILocationFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetLocationById)}/{{locationId}}", Handle)
            .WithSummary("Get a location by ID");

        private static async Task<IResult> Handle(int locationId, ILocationService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.GetLocationByIdRaw(locationId, ct);
                return ApiResponseHelper.Convert(true, true, "Success", 200, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ApiResponseHelper.Convert(false, false, "Error", 500, null);
            }
        }
    }
}
