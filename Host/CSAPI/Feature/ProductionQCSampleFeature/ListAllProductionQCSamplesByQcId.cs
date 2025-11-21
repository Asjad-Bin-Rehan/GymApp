using BS.Services.ProductionQCSampleService.DTOs;
using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Microsoft.AspNetCore.Mvc;
using Logger;
using BS.Services.ProductionQCSampleService;
using Helpers.CustomExceptionThrower;

namespace CSAPI.Feature.ProductionQCSampleFeature
{
    public class ListAllProductionQCSamplesByQcId : IProductionQCSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllProductionQCSamplesByQcId)}", Handle)
            .WithSummary("List all Production QC Samples by QC Id")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseProductionQCSample>>();

        private static async Task<IResult> Handle([FromQuery] string productionQcId, [FromQuery] int? lastCount, [FromQuery] int? skipRecords, ICustomLogger logger, IProductionQCSampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListAllProductionQCSamplesByQcId(productionQcId, ct, lastCount ?? int.MaxValue, skipRecords ?? 0);
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
