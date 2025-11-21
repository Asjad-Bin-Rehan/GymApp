using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.AuthService;
using BS.Services.AuthService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AuthMonolithicFeature
{
    public class Login : IAuthFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(Login)}", Handle)
            .WithSummary("Login a user")
            .WithRequestValidation<RequestLogin>()
            .Produces(200)
            .Produces(404)
            .Produces(400)
            .Produces(403)
            .Produces(422)
            .Produces(500)
            .Produces(HTTPStatusCode400.UnprocessableEntity)
            .Produces<ResponseAuthorizedUser>();

        public class RequestValidator : AbstractValidator<RequestLogin>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        private static async Task<IResult> Handle([FromBody] RequestLogin request, IAuthService svc, ICustomLogger _logger, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.Login(request, ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
            catch (Exception e)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                _logger.LogError(message, e);
                message = ExceptionMessage.SWW;
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}