using BS.Services.AccessLogService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AccessLogFeature
{
    public class DeleteAccessLog : IAccessLogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapDelete($"/{nameof(DeleteAccessLog)}/{{logId}}", Handle)
            .WithSummary("Delete an access log by ID")
            .Produces<bool>()
            .Produces(200)
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(int logId, IAccessLogService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.DeleteAccessLogRaw(logId, ct);
                return ApiResponseHelper.Convert(true, true, "Deleted", 200, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
