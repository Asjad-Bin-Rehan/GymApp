using BS.Services.PurchaseQCSampleService.DTOs;
using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Microsoft.AspNetCore.Mvc;
using Logger;
using BS.Services.PurchaseQCSampleService;
using Helpers.CustomExceptionThrower;

namespace CSAPI.Feature.PurchaseQCSampleFeature
{
    public class GetPurchaseQcSampleById : IPurchaseQCSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetPurchaseQcSampleById)}", Handle)
            .WithSummary("Get Purchase QC Sample by Id")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponsePurchaseQCSampleDTO>();

        private static async Task<IResult> Handle([FromQuery] string purchaseQCSampleId, ICustomLogger logger, IPurchaseQCSampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetPurchaseQCSampleById(ct, purchaseQCSampleId);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentFalseException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
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
