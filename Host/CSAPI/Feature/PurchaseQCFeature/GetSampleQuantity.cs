using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.PurchaseQCService;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PurchaseQCFeature
{
    public class GetSampleQuantity : IPurchaseQCFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetSampleQuantity)}", Handle)
            .WithSummary("Get Sample Quantity by an Inspection Quantity for an Item")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<double>();

        private static async Task<IResult> Handle([FromQuery] int inspectionQuantity, [FromQuery] string itemId, ICustomLogger logger, IPurchaseQCService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetSampleQuantity(itemId, inspectionQuantity, ct);
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
