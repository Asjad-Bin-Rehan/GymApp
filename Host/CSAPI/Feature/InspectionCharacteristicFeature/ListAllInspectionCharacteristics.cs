using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.InspectionCharactersticService;
using BS.Services.InspectionCharactersticService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.InspectionCharacteristicFeature
{
    public class ListAllInspectionCharacteristics : IInspectionCharacteristicFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllInspectionCharacteristics)}", Handle)
            .WithSummary("List all Inspection Characteristics")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseInspectionCharacteristic>>();

        private static async Task<IResult> Handle([FromQuery] int? lastCount, [FromQuery] int? skipRecords, ICustomLogger logger, IInspectionCharacteristicService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListAllInspectionCharacteristics(ct, lastCount ?? int.MaxValue, skipRecords ?? 0);
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
