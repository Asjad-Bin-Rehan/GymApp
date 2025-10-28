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
    public class GetProductionQACavityById : IProductionQACavityFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetProductionQACavityById)}", Handle)
            .WithSummary("Get Production QA Cavity by Id")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponseProductionQACavity>();

        private static async Task<IResult> Handle([FromQuery] string id, ICustomLogger logger, IProductionQACavityService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetProductionQACavityById(id, ct);
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
