using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.InspectionCardService.DTOs;
using BS.Services.InspectionCardService;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;

namespace CSAPI.Feature.InspectionCardFeature
{
    public class GetInspectionCardById : IInspectionCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetInspectionCardById)}", Handle)
            .WithSummary("Get Inspection Cards By Id")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseInspectionCard>>();

        private static async Task<IResult> Handle(string inspectionCardId, ICustomLogger logger, IInspectionCardService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";


            try
            {
                var result = await svc.GetInspectionCardById(ct, inspectionCardId);
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
