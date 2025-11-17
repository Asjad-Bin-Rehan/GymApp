using BS.Services.AccessLogService;
using BS.Services.AccessLogService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AccessLogFeature
{
    public class GetAccessLogsByUserId : IAccessLogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetAccessLogsByUserId)}/{{userId}}", Handle)
            .WithSummary("Get all access logs for a specific user")
            .Produces<List<ResponseAccessLogDTO>>()
            .Produces(200)
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(
            int userId,
            IAccessLogService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var logs = await svc.GetAccessLogsByUserIdRaw(userId, ct);

                if (logs == null || logs.Count == 0)
                    return ApiResponseHelper.Convert(true, false, "No logs found", 404, null);

                return ApiResponseHelper.Convert(true, true, "Access logs retrieved", 200, logs);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
