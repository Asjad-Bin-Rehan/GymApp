using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CustomerProfileService;
using BS.Services.CustomerProfileService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.CustomerProfileFeature
{
    public class UpsertCustomerProfile : ICustomerProfileFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpsertCustomerProfile)}", Handle)
            .WithSummary("Adds or Update a CustomerProfile")
            .Produces<UpsertCustomerProfileResponse>(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode500.InternalServerError)
            ;

        public class RequestValidator : AbstractValidator<UpsertCustomerProfileRequest>
        {

        }

        private static async Task<IResult> Handle([FromBody] UpsertCustomerProfileRequest req, ICustomerProfileService svc, IUserContext user, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.UpsertCustomerProfile(req, user.Data.UserId ?? "Anonymous", ct);
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
