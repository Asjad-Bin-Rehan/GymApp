using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQACavitySampleService.DTOs;
using BS.Services.ProductionQACavitySampleService;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ProductionQACavitySampleFeature
{
    public class ListAllProductionQACavitySamplesByCavityId : IProductionQACavitySampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllProductionQACavitySamplesByCavityId)}", Handle)
            .WithSummary("List all Production QA Cavity Samples by Cavity Id")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseProductionQACavitySample>>();

        private static async Task<IResult> Handle([FromQuery] string cavityId, [FromQuery] int? lastCount, [FromQuery] int? skipRecords, ICustomLogger logger, IProductionQACavitySampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListAllProductionQACavitySamplesByCavityId(cavityId, ct, lastCount ?? int.MaxValue, skipRecords ?? 0);
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
