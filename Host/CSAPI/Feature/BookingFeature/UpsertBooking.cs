using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.BookingService;
using BS.Services.BookingService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.BookingFeature
{
    public class UpsertBooking : IBookingFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpsertBooking)}", Handle)
            .WithSummary("Adds or Update a Booking")
            .Produces<UpsertBookingResponse>(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode500.InternalServerError)
            ;

        public class RequestValidator : AbstractValidator<UpsertBookingRequest>
        {
            
        }

        private static async Task<IResult> Handle([FromBody] UpsertBookingRequest req, IBookingService svc, IUserContext user, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.UpsertBooking(req, user.Data.UserId ?? "Anonymous", ct);
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