using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.NextIntCodeService;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.NextIntCodeFeature
{
    public class GetQualityStatus : INextIntCodeFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
        .MapGet($"/{nameof(GetQualityStatus)}", Handle)
        .WithSummary("Get Quality Status of an entity in API")
        .Produces(200)
        .Produces(404)
        .Produces(500)
        .Produces<int>();

        private static async Task<IResult> Handle([FromQuery] string entityName, [FromQuery] string itemCode, [FromQuery] string? docNumber, [FromQuery] int? lineNo, [FromQuery] string? stageType, ICustomLogger logger, INextIntCodeService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetQualityStatus(entityName, itemCode, docNumber, lineNo, stageType, ct);
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
