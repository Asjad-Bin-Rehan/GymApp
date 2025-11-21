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
    public class ListAllInspectionCards : IInspectionCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllInspectionCards)}", Handle)
            .WithSummary("List all Inspection Cards")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseInspectionCard>>();

        private static async Task<IResult> Handle([FromQuery] int? lastCount, [FromQuery] int? skipRecords, ICustomLogger logger, IInspectionCardService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListAllInspectionCards(ct, lastCount ?? int.MaxValue, skipRecords ?? 0);
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