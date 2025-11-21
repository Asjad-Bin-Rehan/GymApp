using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.NextIntCodeService;

namespace CSAPI.Feature.NextIntCodeFeature
{
    public class GetNextIntCount : INextIntCodeFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
        .MapGet($"/{nameof(GetNextIntCount)}", Handle)
        .WithSummary("Get Next IntCount of an entity in API")
        .Produces(200)
        .Produces(404)
        .Produces(500)
        .Produces<int>();

        private static async Task<IResult> Handle([FromQuery] string entityName, ICustomLogger logger, INextIntCodeService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetNextIntCount(entityName, ct);
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
