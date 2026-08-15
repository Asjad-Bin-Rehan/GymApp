using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.PricingService;
using BS.Services.PricingService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.Auth.Middlewares;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PricingFeature
{
    public class GetPricing : IPricingFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(GetPricing)}", Handle)
            .WithSummary("Get a Pricing")
            .Produces<GetPricingResponse>(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode400.NotFound)
            .Produces(HTTPStatusCode500.InternalServerError)
            ;

        private static async Task<IResult> Handle([FromQuery] string? pricingId, [FromQuery] string? courtId, [FromQuery] string? day, [FromQuery] int lastCount, [FromQuery] int skipRecords, IPricingService svc, IUserContext user, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.GetPricing(pricingId, courtId, day, lastCount, skipRecords, ct);
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
