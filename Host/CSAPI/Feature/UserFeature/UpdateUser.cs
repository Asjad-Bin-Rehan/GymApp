using BS.Services.UserService;
using BS.Services.UserService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.UserFeature
{
    public class UpdateUser : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdateUser)}", Handle)
            .WithSummary("Update user details")
            .Produces<bool>()
            .Produces(404)
            .Produces(500)
            .WithRequestValidation<UpdateUserDTO>();

        public class RequestValidator : AbstractValidator<UpdateUserDTO>
        {
            public RequestValidator()
            {
                RuleFor(x => x.user_id).GreaterThan(0).WithMessage("UserId must be greater than 0.");
                RuleFor(x => x.full_name).NotEmpty().When(x => x.full_name != null).WithMessage("FullName cannot be empty.");
                RuleFor(x => x.email).EmailAddress().When(x => x.email != null).WithMessage("Invalid email format.");
            }
        }

        private static async Task<IResult> Handle(
            [FromBody] UpdateUserDTO request,
            IUserService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var result = await svc.UpdateUserRaw(request, ct);
                return ApiResponseHelper.Convert(true, true, "User updated successfully", 200, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
