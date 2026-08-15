using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.LocationHasLocationService;
using BS.Services.LocationHasLocationService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.LocationFromLocationFeature
{
    public class UpsertLocationFromLocation : ILocationFromLocationFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpsertLocationFromLocation)}", Handle)
            .WithSummary("Adds or Update a LocationFromLocation")
            .Produces<UpsertLocationFromLocationResponse>(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode500.InternalServerError)
            ;

        public class RequestValidator : AbstractValidator<UpsertLocationFromLocationRequest>
        {

        }

        private static async Task<IResult> Handle([FromBody] UpsertLocationFromLocationRequest req, ILocationFromLocationService svc, IUserContext user, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.UpsertLocationFromLocation(req, user.Data.UserId ?? "Anonymous", ct);
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
