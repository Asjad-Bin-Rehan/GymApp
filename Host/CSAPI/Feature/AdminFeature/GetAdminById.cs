using BS.Services.AdminService;
using BS.Services.AdminService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AdminFeature
{
    public class GetAdminById : IAdminFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetAdminById)}/{{adminId:int}}", Handle)
            .WithSummary("Get admin details by adminId")
            .Produces<AdminDTO>()
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromRoute] int adminId,
            IAdminService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            int statusCode = 200;
            string message = "Admin retrieved successfully";

            try
            {
                // --- VALIDATION ---
                if (adminId <= 0)
                    return ApiResponseHelper.Convert(false, false, "Invalid adminId", 400, null);

                // --- SERVICE CALL ---
                var admin = await svc.GetAdminById(adminId, ct);

                if (admin == null)
                    return ApiResponseHelper.Convert(false, false, "Admin not found", 404, null);

                return ApiResponseHelper.Convert(true, true, message, statusCode, admin);
            }
            catch (Exception ex)
            {
                statusCode = 500;
                message = "Something went wrong";
                logger.LogError(ex, ex.Message);

                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
