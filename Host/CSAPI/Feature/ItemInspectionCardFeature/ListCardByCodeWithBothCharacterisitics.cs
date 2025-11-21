using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ItemInspectionCardService.DTOs;
using BS.Services.ItemInspectionCardService;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ItemInspectionCardFeature
{
    public class ListCardByCodeWithBothCharacteristics : IItemInspectionCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListCardByCodeWithBothCharacteristics)}", Handle)
            .WithSummary("List Cards By Code With Both Qualitative and Quantitative Characteristics")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseCardWithBothCharacteristicsDTO>>();

        private static async Task<IResult> Handle([FromQuery] string itemCode, ICustomLogger logger, IItemInspectionCardService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListCardByCodeWithBothCharacteristics(itemCode, ct);
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
