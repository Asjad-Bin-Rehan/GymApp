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
                // Validate inputs
                if (string.IsNullOrWhiteSpace(request.username) ||
                    string.IsNullOrWhiteSpace(request.password))
                {
                    return ApiResponseHelper.Convert(false, false,
                        "Username and password are required", 400, null);
                }

                // Call service
                var adminId = await svc.SignUpAdmin(request, ct);

                // Success response
                return ApiResponseHelper.Convert(true, true, "Admin created successfully", 200, new
                {
                    admin_id = adminId,
                    username = request.username,
                    role = request.role,
                    full_name = request.full_name,
                    phone = request.phone,
                    date_of_birth = request.date_of_birth,
                    email = request.email,
                    join_date = DateTime.UtcNow.Date
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                // Validation failures from service
                if (ex.Message == "USERNAME_EXISTS")
                {
                    return ApiResponseHelper.Convert(false, false,
                        "Username already exists", 400, null);
                }

                if (ex.Message == "EMAIL_EXISTS")
                {
                    return ApiResponseHelper.Convert(false, false,
                        "Email already exists", 400, null);
                }

                // Unknown internal failure
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
