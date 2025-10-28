using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.QualitativeResultService.DTOs;
using BS.Services.QualitativeResultService;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;

namespace CSAPI.Feature.QualitativeResultFeature
{
    public class GetQualitativeResultById : IQualitativeResultFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
           .MapGet($"/{nameof(GetQualitativeResultById)}", Handle)
           .WithSummary("Get Qualitative Result By Id")
           .Produces(200)
           .Produces(404)
           .Produces(500)
           .Produces<List<ResponseQualitativeResult>>();

        private static async Task<IResult> Handle(string id, ICustomLogger logger, IQualitativeResultService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetQualitativeResultById(id, ct);
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
