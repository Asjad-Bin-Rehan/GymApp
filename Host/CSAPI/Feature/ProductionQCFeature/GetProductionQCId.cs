using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.ProductionQCService.DTOs;
using BS.Services.ProductionQCService;

namespace CSAPI.Feature.ProductionQCFeature
{
    public class GetProductionQCId : IProductionQCFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetProductionQCId)}", Handle)
            .WithSummary("Get Production QC ID by ItemCode and DocNumber")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponseProductionQCWithItem>();

        private static async Task<IResult> Handle([FromQuery] string itemCode, [FromQuery] string? docNumber, [FromQuery] string? stageType, ICustomLogger logger, IProductionQCService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetProductionQCId(itemCode, docNumber, stageType, ct);
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
