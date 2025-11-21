using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.NextIntCodeService;

namespace CSAPI.Feature.NextIntCodeFeature
{
    public class GetTotalRecordsCount : INextIntCodeFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
        .MapGet($"/{nameof(GetTotalRecordsCount)}", Handle)
        .WithSummary("Get Total Records for Pagination")
        .Produces(200)
        .Produces(404)
        .Produces(500)
        .Produces<int>();

        private static async Task<IResult> Handle([FromQuery] string entityName, [FromQuery] string? filterParameter, ICustomLogger logger, INextIntCodeService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetTotalRecordsCount(entityName, filterParameter, ct);
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
