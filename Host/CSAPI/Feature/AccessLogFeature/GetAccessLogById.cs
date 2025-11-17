using BS.Services.AccessLogService;
using BS.Services.AccessLogService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AccessLogFeature
{
    public class GetAccessLogById : IAccessLogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetAccessLogById)}", Handle)
            .WithSummary("Get an access log by ID")
            .Produces<ResponseAccessLogDTO>()
            .Produces(200)
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle([FromQuery] int logId, IAccessLogService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.GetAccessLogByIdRaw(logId, ct);
                if (result == null)
                    return ApiResponseHelper.Convert(true, false, "Not found", 404, null);

                return ApiResponseHelper.Convert(true, true, "Success", 200, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
