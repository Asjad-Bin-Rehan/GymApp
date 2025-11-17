using BS.Services.AccessLogService;
using BS.Services.AccessLogService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AccessLogFeature
{
    public class ListAllAccessLogs : IAccessLogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllAccessLogs)}", Handle)
            .WithSummary("List access logs (paginated)")
            .Produces<List<ResponseAccessLogDTO>>()
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle([FromQuery] int limit, [FromQuery] int offset, IAccessLogService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.ListAllAccessLogsRaw(limit <= 0 ? 100 : limit, offset < 0 ? 0 : offset, ct);
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
