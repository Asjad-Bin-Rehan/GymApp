using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.PurchaseQCService;
using BS.Services.PurchaseQCService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PurchaseQCFeature
{
    public class GetPurchaseQCWithItemById : IPurchaseQCFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetPurchaseQCWithItemById)}", Handle)
            .WithSummary("Get Purchase QC with Item by Id")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponsePurchaseQCWithItem>();

        private static async Task<IResult> Handle([FromQuery] string qcId, ICustomLogger logger, IPurchaseQCService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetPurchaseQCWithItemById(qcId, ct);
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
                logger.LogError(e.Message);
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
