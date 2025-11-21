using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ItemCardService.DTOs;
using BS.Services.ItemCardService;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;

namespace CSAPI.Feature.ItemCardFeature
{
    public class GetItemByCode : IItemCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
           .MapGet($"/{nameof(GetItemByCode)}", Handle)
           .WithSummary("Get Item By Code or DocNo & LineNo")
           .Produces(200)
           .Produces(404)
           .Produces(500)
           .Produces<List<ResponseItemCard>>();

        private static async Task<IResult> Handle(string itemCode, string? docNum, string? lineNum, ICustomLogger logger, IItemCardService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";


            try
            {
                var result = await svc.GetItemByCode(ct, itemCode, docNum, lineNum);
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
