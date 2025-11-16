using BS.Services.UserService;
using BS.Services.UserService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.UserFeature
{
    public class GetUserById : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetUserById)}", Handle)
            .WithSummary("Gets a user by ID")
            .Produces<ResponseUserDTO>()
            .Produces(200)
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle([FromQuery] int userId, IUserService svc, ICustomLogger logger, CancellationToken ct)
        {
            int statusCode = 200;
            string message = "Success";

            try
            {
                var user = await svc.GetUserByIdRaw(userId, ct);
                if (user == null)
                {
                    statusCode = 404;
                    message = "User not found";
                    return ApiResponseHelper.Convert(false, false, message, statusCode, null);
                }

                return ApiResponseHelper.Convert(true, true, message, statusCode, user);
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
