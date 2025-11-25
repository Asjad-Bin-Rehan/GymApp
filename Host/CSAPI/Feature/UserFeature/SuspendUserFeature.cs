using BS.Services.UserService;
using BS.Services.UserService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Logger;
using Microsoft.AspNetCore.Mvc;
using static BS.Services.UserService.UserService;

namespace CSAPI.Feature.UserFeature
{
    public class SuspendUser : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(SuspendUser)}", Handle)
            .WithSummary("Suspend a user (SuperAdmin only)")
            .Produces<bool>()
            .Produces(400)
            .Produces(401)
            .Produces(404)
            .Produces(500)
            .WithRequestValidation<SuspendUserDTO>();

        public class RequestValidator : AbstractValidator<SuspendUserDTO>
        {
            public RequestValidator()
            {
                RuleFor(x => x.user_id).GreaterThan(0).WithMessage("UserId must be greater than 0.");
                RuleFor(x => x.admin_id).GreaterThan(0).WithMessage("AdminId must be greater than 0.");
            }
        }

        private static async Task<IResult> Handle(
            [FromBody] SuspendUserDTO request,
            IUserService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var result = await svc.SuspendUserRaw(request, ct);

                return result switch
                {
                    SuspendUserResult.InvalidRequest =>
                        ApiResponseHelper.Convert(false, false, "Invalid request body", 400, null),

                    SuspendUserResult.InvalidUserId =>
                        ApiResponseHelper.Convert(false, false, "Invalid user ID", 400, null),

                    SuspendUserResult.InvalidAdminId =>
                        ApiResponseHelper.Convert(false, false, "Invalid admin ID", 400, null),

                    SuspendUserResult.AdminNotFound =>
                        ApiResponseHelper.Convert(false, false, "Admin not found", 404, null),

                    SuspendUserResult.Unauthorized =>
                        ApiResponseHelper.Convert(false, false, "Only SuperAdmins can suspend users", 401, null),

                    SuspendUserResult.UserNotFound =>
                        ApiResponseHelper.Convert(false, false, "User not found", 404, null),

                    SuspendUserResult.Success =>
                        ApiResponseHelper.Convert(true, true, "User suspended successfully", 200, null),

                    _ =>
                        ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null),
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ApiResponseHelper.Convert(false, false, "Internal server error", 500, null);
            }
        }
    }
}
