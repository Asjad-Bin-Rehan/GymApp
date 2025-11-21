using BS.Services.AdminService;
using BS.Services.AdminService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AdminFeature
{
    public class ListAllAdmins : IAdminFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllAdmins)}", Handle)
            .WithSummary("List all admins")
            .Produces<List<AdminDTO>>()
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle(
            IAdminService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            int statusCode = 200;
            string message = "Success";

            try
            {
                var admins = await svc.ListAllAdmins(ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, admins);
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
