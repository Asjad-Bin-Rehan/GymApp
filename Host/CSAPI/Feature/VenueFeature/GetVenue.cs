using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.VenueService;
using BS.Services.VenueService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.Auth.Middlewares;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.VenueFeature
{
    public class GetVenue : IVenueFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(GetVenue)}", Handle)
            .WithSummary("Get a Venue")
            .Produces<GetVenueResponse>(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode400.NotFound)
            .Produces(HTTPStatusCode500.InternalServerError)
            ;

        private static async Task<IResult> Handle([FromQuery] string? venueId, [FromQuery] string? organizationId, [FromQuery] string? locationId, [FromQuery] string? name, [FromQuery] int lastCount, [FromQuery] int skipRecords, IVenueService svc, IUserContext user, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.GetVenue(venueId, organizationId, locationId, name, lastCount, skipRecords, ct);
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
