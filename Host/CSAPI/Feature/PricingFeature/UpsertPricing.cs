using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.PricingService;
using BS.Services.PricingService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PricingFeature
{
    public class UpsertPricing : IPricingFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpsertPricing)}", Handle)
            .WithSummary("Adds or Update a Pricing")
            .Produces<UpsertPricingResponse>(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode500.InternalServerError)
            ;

        public class RequestValidator : AbstractValidator<UpsertPricingRequest>
        {

        }

        private static async Task<IResult> Handle([FromBody] UpsertPricingRequest req, IPricingService svc, IUserContext user, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.UpsertPricing(req, user.Data.UserId ?? "Anonymous", ct);
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
