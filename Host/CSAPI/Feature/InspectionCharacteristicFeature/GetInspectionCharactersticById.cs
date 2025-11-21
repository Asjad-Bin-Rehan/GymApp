using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using BS.Services.InspectionCharactersticService.DTOs;
using BS.Services.InspectionCharactersticService;


namespace CSAPI.Feature.InspectionCharacteristicFeature
{
    public class GetInspectionCharacteristicById : IInspectionCharacteristicFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetInspectionCharacteristicById)}", Handle)
            .WithSummary("Get Inspection Characteristics By Id")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<List<ResponseInspectionCharacteristic>>();

        private static async Task<IResult> Handle(string inspectionCharId, ICustomLogger logger, IInspectionCharacteristicService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";

            try
            {
                var result = await svc.GetInspectionCharacteristicById(ct, inspectionCharId);
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
