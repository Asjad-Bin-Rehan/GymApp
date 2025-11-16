using BS.Services.UserService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.UserFeature
{
    public class DeleteUser : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapDelete($"/{nameof(DeleteUser)}", Handle)
            .WithSummary("Deletes a user by ID")
            .Produces<bool>()
            .Produces(200)
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle([FromQuery] int userId, IUserService svc, ICustomLogger logger, CancellationToken ct)
        {
            int statusCode = 200;
            string message = "User deleted successfully";

            try
            {
                var success = await svc.DeleteUserRaw(userId, ct);
                if (!success)
                {
                    statusCode = 404;
                    message = "User not found";
                    return ApiResponseHelper.Convert(false, false, message, statusCode, null);
                }

                return ApiResponseHelper.Convert(true, true, message, statusCode, true);
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
