using BS.Services.AdminService;
using BS.Services.AdminService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AdminFeature
{
    public class SignUpAdmin : IFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("/signup-admin", Handle)
               .WithSummary("Sign up a new admin manually")
               .Produces(200)
               .Produces(400)
               .Produces(500);
        }

        private static async Task<IResult> Handle(
            [FromBody] SignUpAdminDTO request,
            [FromServices] IAdminService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.username) || string.IsNullOrWhiteSpace(request.password))
                {
                    return ApiResponseHelper.Convert(false, false, "Username and password are required", 400, null);
                }

                var adminId = await svc.SignUpAdmin(request, ct);

                return ApiResponseHelper.Convert(true, true, "Admin created successfully", 200, new { admin_id = adminId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
