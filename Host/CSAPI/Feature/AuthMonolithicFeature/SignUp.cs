using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.AuthService.DTOs;
using BS.Services.AuthService;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AuthMonolithicFeature
{
    public class SignUp : IAuthFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(SignUp)}", Handle)
            .WithSummary("sign up a user")
            .WithRequestValidation<RequestSignUp>()
            .Produces(201)
            .Produces(HTTPStatusCode400.NotAcceptable)
            .Produces(HTTPStatusCode400.BadRequest)
            .Produces(500)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestSignUp>
        {
            public RequestValidator()
            {
                
            }
        }

        private static async Task<IResult> Handle([FromBody] RequestSignUp request, [FromHeader] string? DeviceId, IAuthService _auth, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await _auth.SignUp(request, DeviceId ?? "Anonymous", cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentException e)
            {
                statusCode = HTTPStatusCode400.NotAcceptable;
                message = e.Message;
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
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
