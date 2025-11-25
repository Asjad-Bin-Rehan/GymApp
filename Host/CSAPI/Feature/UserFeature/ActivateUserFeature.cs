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
    public class ActivateUser : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(ActivateUser)}", Handle)
            .WithSummary("Activate a suspended user (SuperAdmin only)")
            .Produces<bool>()
            .Produces(400)
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
                var result = await svc.ActivateUserRaw(request, ct);

                return result switch
                {
                    ActivateUserResult.InvalidRequest =>
                        ApiResponseHelper.Convert(false, false, "Invalid request", 400, null),

                    ActivateUserResult.InvalidUserId =>
                        ApiResponseHelper.Convert(false, false, "Invalid user ID", 400, null),

                    ActivateUserResult.InvalidAdminId =>
                        ApiResponseHelper.Convert(false, false, "Invalid admin ID", 400, null),

                    ActivateUserResult.AdminNotFound =>
                        ApiResponseHelper.Convert(false, false, "Admin not found", 404, null),

                    ActivateUserResult.Unauthorized =>
                        ApiResponseHelper.Convert(false, false, "Only SuperAdmin can activate users", 400, null),

                    ActivateUserResult.UserNotFound =>
                        ApiResponseHelper.Convert(false, false, "User not found", 404, null),

                    ActivateUserResult.Failed =>
                        ApiResponseHelper.Convert(false, false, "Activation failed", 500, null),

                    ActivateUserResult.Success =>
                        ApiResponseHelper.Convert(true, true, "User activated successfully", 200, true),

                    _ =>
                        ApiResponseHelper.Convert(false, false, "Unknown error", 500, null)
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
