using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ItemSampleService;
using BS.Services.ItemSampleService.DTO;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ItemSampleFeature
{
    public class ListRangesBySampleId : IItemSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListRangesBySampleId)}", Handle)
            .WithSummary("List all Ranges by Sample ID")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseItemSampleWithSamplingRanges>>();

        private static async Task<IResult> Handle([FromQuery] string sampleId, [FromQuery] int? lastCount, [FromQuery] int? skipRecords, ICustomLogger logger, IItemSampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListRangesBySampleId(sampleId, ct, lastCount ?? int.MaxValue, skipRecords ?? 0);
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