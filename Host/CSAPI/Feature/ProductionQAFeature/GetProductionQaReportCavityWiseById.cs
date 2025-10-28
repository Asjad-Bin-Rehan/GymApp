using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQAService.DTOs;
using BS.Services.ProductionQAService;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ProductionQAFeature
{
    public class GetProductionQaReportCavityWiseById : IProductionQAFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetProductionQaReportCavityWiseById)}", Handle)
            .WithSummary("Get Production QA Report By ID")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponseProductionQaReportCavityWise>();

        private static async Task<IResult> Handle([FromQuery] string qaId, ICustomLogger logger, IProductionQAService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetProductionQaReportCavityWiseById(qaId, ct);
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
