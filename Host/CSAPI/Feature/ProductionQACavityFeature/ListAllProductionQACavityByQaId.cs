using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQACavityService;
using BS.Services.ProductionQACavityService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ProductionQACavityFeature
{
    public class ListAllProductionQACavityByQaId : IProductionQACavityFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllProductionQACavityByQaId)}", Handle)
            .WithSummary("List all Production QA Cavities")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseProductionQACavity>>();

        private static async Task<IResult> Handle([FromQuery] int? lastCount, [FromQuery] string qaId, [FromQuery] int? skipRecords, ICustomLogger logger, IProductionQACavityService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListAllProductionQACavityByQaId(ct, qaId, lastCount ?? int.MaxValue, skipRecords ?? 0);
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
