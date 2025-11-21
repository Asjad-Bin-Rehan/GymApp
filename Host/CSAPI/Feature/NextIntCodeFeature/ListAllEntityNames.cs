using BS.Services.NextIntCodeService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using BS.CustomExceptions.CustomExceptionMessage;

namespace CSAPI.Feature.NextIntCodeFeature
{
    public class ListAllEntityNames : INextIntCodeFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
        .MapGet($"/{nameof(ListAllEntityNames)}", Handle)
        .WithSummary("List All Entity Names")
        .Produces(200)
        .Produces(500)
        .Produces<List<string>>();

        private static async Task<IResult> Handle(ICustomLogger logger, INextIntCodeService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListAllEntityNamesAsync();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
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