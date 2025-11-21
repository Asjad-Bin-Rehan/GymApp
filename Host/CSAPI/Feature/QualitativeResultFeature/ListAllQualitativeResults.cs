using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.QualitativeResultService;
using BS.Services.QualitativeResultService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.QualitativeResultFeature
{
    public class ListAllQualitativeResults : IQualitativeResultFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllQualitativeResults)}", Handle)
            .WithSummary("List All Qualitative Results")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseQualitativeResult>>();

        private static async Task<IResult> Handle([FromQuery] int? lastCount, [FromQuery] int? skipRecords, ICustomLogger logger, IQualitativeResultService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListAllQualitativeResults(ct, lastCount ?? int.MaxValue, skipRecords ?? 0);
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