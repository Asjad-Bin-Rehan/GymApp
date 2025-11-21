using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.InspectionCardService.DTOs;
using BS.Services.InspectionCardService;

namespace CSAPI.Feature.InspectionCardFeature
{
    public class ListCharacteristicsByInspectionCardId : IInspectionCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListCharacteristicsByInspectionCardId)}", Handle)
            .WithSummary("List all Inspection Characteristics by Card ID")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseCardWithCharacteristics>>();

        private static async Task<IResult> Handle([FromQuery] string inspectionCardId, [FromQuery] int? lastCount, [FromQuery] int? skipRecords, ICustomLogger logger, IInspectionCardService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListAllCharacteristicsByInspectionCardId(inspectionCardId, ct, lastCount ?? int.MaxValue, skipRecords ?? 0);
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
