using BS.Services.AuthService;
using CSAPI.Common;
using CustomHTTP;
using Helpers.Auth.JWT;
using Helpers.Auth.Models;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AuthMonolithicFeature
{
    public class RefreshToken : IAuthFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(RefreshToken)}", Handle)
            .Produces(200)
            .Produces(401)
            .WithSummary("refreshToken to get access token (JWT)");

        private static async Task<IResult> Handle([FromBody] AccessAndRefreshTokens request, IAuthService service, Jwt jwt, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await service.GetRefreshToken(request, cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (UnauthorizedAccessException e)
            {
                statusCode = HTTPStatusCode400.Unauthorized;
                message = e.Message;
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (Exception e)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
