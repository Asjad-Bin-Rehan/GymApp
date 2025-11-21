using BS.Services.AccessLogService;
using BS.Services.AccessLogService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AccessLogFeature
{
    public class GetAccessLogsByGymId : IAccessLogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetAccessLogsByGymId)}/{{gymId}}", Handle)
            .WithSummary("Get all access logs for a specific gym")
            .Produces<List<ResponseAccessLogDTO>>()
            .Produces(200)
            .Produces(404)
            .Produces(400)
            .Produces(500);

        private static async Task<IResult> Handle(
            int gymId,
            IAccessLogService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                // --- VALIDATION ---
                if (gymId <= 0)
                    return ApiResponseHelper.Convert(true, false, "Invalid gymId", 400, null);

                // Optional: Check if gym exists
                var gymExists = await svc.GymExists(gymId, ct);
                if (!gymExists)
                    return ApiResponseHelper.Convert(true, false, "Gym does not exist", 404, null);

                // Fetch logs
                var logs = await svc.GetAccessLogsByGymIdRaw(gymId, ct);

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
