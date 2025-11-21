using BS.Services.PurchaseQCSampleService.DTOs;
using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.PurchaseQCSampleService;
using CSAPI.Extensions.RouteHandler;

namespace CSAPI.Feature.PurchaseQCSampleFeature
{
    public class UpdatePurchaseQcSample : IPurchaseQCSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdatePurchaseQcSample)}", Handle)
            .WithSummary("Update Purchase QC Sample")
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<UpdatePurchaseQCSampleDTO>()
            .Produces<bool>();

        private static async Task<IResult> Handle([FromBody] UpdatePurchaseQCSampleDTO req, IUserContext user, ICustomLogger logger, IPurchaseQCSampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.UpdatePurchaseQCSample(req, user.Data.UserId ?? "Anonymous", ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                return ApiResponseHelper.Convert(true, false, message, statusCode, false);
            }
            catch (Exception e)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                logger.LogError(e.Message);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
