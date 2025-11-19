using BS.Services.AdminService;
using BS.Services.AdminService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AdminFeature
{
    public class LoginAdminWithoutJWT
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(LoginAdminWithoutJWT)}", Handle)
            .WithSummary("Login admin without JWT (simple validation)")
            .Produces(200)
            .Produces(400)
            .Produces(401)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromBody] LoginAdminWithoutJWTDTO request, // <- use the correct DTO
            [FromServices] IAdminService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return ApiResponseHelper.Convert(false, false, "Username and password are required", 400, null);
                }

                var result = await svc.LoginAdminWithoutJWT(request, ct); // <- result is LoginAdminWithoutJWTResponseDTO

                if (!result.Success)
                    return ApiResponseHelper.Convert(false, false, result.Message, 401, null);

                return ApiResponseHelper.Convert(true, true, result.Message, 200, new
                {
                    admin_id = result.AdminId,
                    username = result.Username,
                    role = result.Role
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
