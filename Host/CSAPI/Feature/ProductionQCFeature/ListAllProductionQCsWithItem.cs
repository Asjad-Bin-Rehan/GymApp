using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQCService;
using BS.Services.ProductionQCService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ProductionQCFeature
{
    public class ListAllProductionQCsWithItem : IProductionQCFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllProductionQCsWithItem)}", Handle)
            .WithSummary("List of all Production QC Records")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseProductionQCWithItem>>();

        private static async Task<IResult> Handle([FromQuery] int? lastCount, [FromQuery] int? skipRecords, ICustomLogger logger, IProductionQCService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                
                var result = await svc.ListAllProductionQCsWithItem(ct, lastCount ?? int.MaxValue, skipRecords ?? 0);
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
