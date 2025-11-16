using BS.Services.LocationService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.LocationFeature
{
    public class ListAllLocations : ILocationFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllLocations)}", Handle)
            .WithSummary("List all locations");

        private static async Task<IResult> Handle(int limit, int offset, ILocationService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.ListAllLocationsRaw(limit, offset, ct);
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
