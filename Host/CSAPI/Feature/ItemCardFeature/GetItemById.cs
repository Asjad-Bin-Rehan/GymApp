using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using BS.Services.ItemCardService.DTOs;
using BS.Services.ItemCardService;

namespace CSAPI.Feature.ItemCardFeature
{
    public class GetItemById : IItemCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetItemById)}", Handle)
            .WithSummary("Get Item By Id")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseItemCard>>();

        private static async Task<IResult> Handle(string itemId, ICustomLogger logger, IItemCardService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";


            try
            {
                var result = await svc.GetItemById(ct, itemId);
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
