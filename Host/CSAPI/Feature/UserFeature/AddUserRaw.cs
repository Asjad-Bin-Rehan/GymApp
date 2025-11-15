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
    public class AddUserRaw : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddUserRaw)}", Handle)
            .WithSummary("Adds a new user (Admin)")
            .Produces<bool>()
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<AddUserDTO>();

        public class RequestValidator : AbstractValidator<AddUserDTO>
        {
            private readonly IUserService _svc;
            public RequestValidator(IUserService svc)
            {
                _svc = svc;

                RuleFor(x => x.Username)
                    .NotEmpty().WithMessage("Username is required.")
                    .MustAsync(async (username, ct) => !await _svc.IsUsernameExistsAsync(username, ct))
                    .WithMessage("Username already exists.");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.")
                    .MustAsync(async (email, ct) => !await _svc.IsEmailExistsAsync(email, ct))
                    .WithMessage("Email already exists.");
            }
        }

        private static async Task<IResult> Handle([FromBody] AddUserDTO request, IUserService svc, ICustomLogger logger, CancellationToken ct)
        {
            int statusCode = 200;
            string message = "User added successfully";

            try
            {
                var result = await svc.AddUserRawAsync(request, ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
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
