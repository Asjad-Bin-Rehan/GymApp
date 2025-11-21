using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using BS.Services.ItemCardService.DTOs;
using BS.Services.ItemCardService;
using Microsoft.AspNetCore.Builder;

namespace CSAPI.Feature.ItemCardFeature
{
    public class GetItemTypesById : IItemCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetItemTypesById)}", Handle)
            .WithSummary("Get Item Types By Id with Dispatch/Incoming Info")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponseItemTypes>();

        private static async Task<IResult> Handle(
            string itemId,
            int lastCount,
            int skipRecords,
            ICustomLogger logger,
            IItemCardService svc,
            CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";

            try
            {
                var result = await svc.GetItemTypesById(itemId, ct, lastCount, skipRecords);
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
