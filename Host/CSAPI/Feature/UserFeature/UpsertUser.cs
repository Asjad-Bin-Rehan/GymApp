using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.UserService;
using BS.Services.UserService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.UserFeature
{
    public class UpsertUser : IUserFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpsertUser)}", Handle)
            .WithSummary("Adds or Update a User")
            .Produces<UpsertUserResponse>(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode500.InternalServerError)
            ;

        public class RequestValidator : AbstractValidator<UpsertUserRequest>
        {

        }

        private static async Task<IResult> Handle([FromBody] UpsertUserRequest req, IUserService svc, IUserContext user, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.UpsertUser(req, user.Data.UserId ?? "Anonymous", ct);
                return ApiResponseHelper.Convert(true, true, nameof(HTTPStatusCode200.Ok), HTTPStatusCode200.Ok, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, ExceptionMessage.SWW, HTTPStatusCode500.InternalServerError, ex.Message);
            }
        }
    }
}
