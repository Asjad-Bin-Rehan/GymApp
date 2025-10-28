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
    public class ListAllPurchaseQCSamplesByQcId : IPurchaseQCSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllPurchaseQCSamplesByQcId)}", Handle)
            .WithSummary("List all Purchase QC Samples by QC Id")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponsePurchaseQCSampleDTO>>();

        private static async Task<IResult> Handle([FromQuery] string purchaseQcId, [FromQuery] int? lastCount, [FromQuery] int? skipRecords, ICustomLogger logger, IPurchaseQCSampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListAllPurchaseQCSamplesByQcId(purchaseQcId, ct, lastCount ?? int.MaxValue, skipRecords ?? 0);
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
