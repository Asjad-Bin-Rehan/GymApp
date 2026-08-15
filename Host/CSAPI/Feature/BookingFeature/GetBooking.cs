using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.BookingService;
using BS.Services.BookingService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.Auth.Middlewares;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.BookingFeature
{
    public class GetBooking : IBookingFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(GetBooking)}", Handle)
            .WithSummary("Get a Booking")
            .Produces<GetBookingResponse>(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode400.NotFound)
            .Produces(HTTPStatusCode500.InternalServerError)
            ;

        private static async Task<IResult> Handle([FromQuery] string? bookingId, [FromQuery] string? slotId, [FromQuery] string? customerProfileId, [FromQuery] string? status, [FromQuery] string? paymentStatus, [FromQuery] string? name, [FromQuery] int lastCount, [FromQuery] int skipRecords, IBookingService svc, IUserContext user, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.GetBooking(bookingId, slotId, customerProfileId, status, paymentStatus, name, lastCount, skipRecords, ct);
                return ApiResponseHelper.Convert(true, true, nameof(HTTPStatusCode200.Ok), HTTPStatusCode200.Ok, result);
            }
            catch (ArgumentFalseException ex)
            {
                return ApiResponseHelper.Convert(true, false, ExceptionMessage.NA, HTTPStatusCode400.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, ExceptionMessage.SWW, HTTPStatusCode500.InternalServerError, ex.Message);
            }
        }
    }
}