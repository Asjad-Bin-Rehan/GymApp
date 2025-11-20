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
    public class LoginUser : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(LoginUser)}", Handle)
            .WithSummary("Logs in a user and returns JWT token")
            .Produces<ResponseUserDTO>()
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<LoginUserDTO>();

        public class RequestValidator : AbstractValidator<LoginUserDTO>
        {
            public RequestValidator()
            {
                RuleFor(x => x.username_or_email)
                    .NotEmpty().WithMessage("Username or Email is required.");

                RuleFor(x => x.password)
                    .NotEmpty().WithMessage("Password is required.");
            }
        }

        private static async Task<IResult> Handle([FromBody] LoginUserDTO request, IUserService svc, ICustomLogger logger, CancellationToken ct)
        {
            int statusCode = 200;
            string message = "Login successful";

            try
            {
                var (user, errorMessage) = await svc.LoginUserRaw(request, ct);

                if (user == null)
                {
                    statusCode = 400;
                    message = errorMessage ?? "Invalid credentials";
                    Console.WriteLine($"❌ LOGIN ENDPOINT: {message}");
                    return ApiResponseHelper.Convert(false, false, message, statusCode, null);
                }

                Console.WriteLine("✅ LOGIN ENDPOINT: Returning user data with token");
                Console.WriteLine($"   Response includes token: {!string.IsNullOrEmpty(user.token)}");

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
