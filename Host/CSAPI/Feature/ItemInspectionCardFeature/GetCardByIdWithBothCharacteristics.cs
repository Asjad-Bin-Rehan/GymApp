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
    public class GetCardByIdWithBothCharacteristics : IItemInspectionCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetCardByIdWithBothCharacteristics)}", Handle)
            .WithSummary("List Card By Id With Both Qualitative and Quantitative Characteristics")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponseCardWithBothCharacteristicsDTO>();

        private static async Task<IResult> Handle([FromQuery] string itemInspectionCardId, ICustomLogger logger, IItemInspectionCardService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetCardByIdWithBothCharacteristics(itemInspectionCardId, ct);
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
