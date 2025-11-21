using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQAService;
using BS.Services.ProductionQAService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ProductionQAFeature
{
    public class GetProductionQAId : IProductionQAFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetProductionQAId)}", Handle)
            .WithSummary("Get Production QA ID by ItemCode and DocNumber")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponseProductionQAWithItem>();

        private static async Task<IResult> Handle([FromQuery] string itemCode, [FromQuery] string? docNumber, [FromQuery] string? stageType, ICustomLogger logger, IProductionQAService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetProductionQAId(itemCode, docNumber, stageType, ct);
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
