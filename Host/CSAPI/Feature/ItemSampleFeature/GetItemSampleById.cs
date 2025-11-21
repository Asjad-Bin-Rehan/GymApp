using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using BS.Services.ItemSampleService;
using BS.Services.ItemSampleService.DTO;

namespace CSAPI.Feature.ItemSampleFeature
{
    public class GetItemSampleById : IItemSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetItemSampleById)}", Handle)
            .WithSummary("Get Item Sample by ID")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponseItemSample>();

        private static async Task<IResult> Handle(string itemSampleId, ICustomLogger logger, IItemSampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetItemSampleById(itemSampleId, ct);
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

