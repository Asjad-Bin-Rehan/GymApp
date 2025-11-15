using BS.Services.UserService;
using BS.Services.UserService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.UserFeature
{
    public class AddUser : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddUser)}", Handle)
            .WithSummary("Register a new user")
            .Produces<bool>()
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<AddUserDTO>();

        public class RequestValidator : AbstractValidator<AddUserDTO>
        {
            private readonly IUserServiceRaw _svc;
            public RequestValidator(IUserServiceRaw svc)
            {
                _svc = svc;

                RuleFor(x => x.Username)
                    .NotEmpty().WithMessage("Username is required.")
                    .MustAsync(IsUsernameNotExist).WithMessage("Username already exists.");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.")
                    .MustAsync(IsEmailNotExist).WithMessage("Email already exists.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
            }

            private async Task<bool> IsUsernameNotExist(string username, CancellationToken ct)
                => !await _svc.IsUsernameExistsRaw(username, ct);

            private async Task<bool> IsEmailNotExist(string email, CancellationToken ct)
                => !await _svc.IsEmailExistsRaw(email, ct);
        }

        private static async Task<IResult> Handle([FromBody] AddUserDTO request, IUserServiceRaw svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.AddUserRaw(request, ct); // Hashing handled inside service
                return ApiResponseHelper.Convert(true, true, "User registered successfully", 200, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
