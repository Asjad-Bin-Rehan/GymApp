using BS.Services.AccessLogService;
using BS.Services.AccessLogService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AccessLogFeature
{
    public class GetAccessLogsByGymId_View : IAccessLogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetAccessLogsByGymId_View)}/{{gymId}}", Handle)
            .WithSummary("Get access logs for a gym using VIEW")
            .Produces<List<ResponseAccessLogViewDTO>>()
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
                // --- Validation ---
                if (gymId <= 0)
                    return ApiResponseHelper.Convert(true, false, "Invalid gymId. Must be greater than zero.", 400, null);

                var gymExists = await svc.GymExists(gymId, ct);
                if (!gymExists)
                    return ApiResponseHelper.Convert(true, false, $"Gym with ID {gymId} does not exist.", 404, null);

                var logs = await svc.GetAccessLogsByGymIdView(gymId, ct);

                if (logs == null || logs.Count == 0)
                    return ApiResponseHelper.Convert(true, false, $"No access logs found for gym ID {gymId}.", 404, null);

                return ApiResponseHelper.Convert(true, true, $"Access logs retrieved successfully for gym ID {gymId}.", 200, logs);
            }
            catch (ArgumentException argEx)
            {
                logger.LogError($"Argument error: {argEx.Message}\n{argEx.StackTrace}");
                return ApiResponseHelper.Convert(false, false, argEx.Message, 400, null);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unexpected error: {ex.Message}\n{ex.StackTrace}");
                return ApiResponseHelper.Convert(false, false, "Something went wrong while fetching access logs.", 500, null);
            }
        }
    }
}
