using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.SlotService;
using BS.Services.SlotService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.Auth.Middlewares;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.SlotFeature
{
    public class GetSlot : ISlotFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(GetSlot)}", Handle)
            .WithSummary("Get a Slot")
            .Produces<GetSlotResponse>(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode400.NotFound)
            .Produces(HTTPStatusCode500.InternalServerError)
            ;

        private static async Task<IResult> Handle([FromQuery] string? slotId, [FromQuery] string? courtId, [FromQuery] string? day, [FromQuery] bool? isAvailable, [FromQuery] int lastCount, [FromQuery] int skipRecords, ISlotService svc, IUserContext user, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var result = await svc.GetSlot(slotId, courtId, day, isAvailable, lastCount, skipRecords, ct);
                return ApiResponseHelper.Convert(true, true, nameof(HTTPStatusCode200.Ok), HTTPStatusCode200.Ok, result);
            }
            catch (ArgumentFalseException ex)
            {
                return ApiResponseHelper.Convert(true, false, ExceptionMessage.NA, HTTPStatusCode400.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, ExceptionMessage.SWW, HTTPStatusCode500.InternalServerError, ex.Message);
            }
        }
    }
}
