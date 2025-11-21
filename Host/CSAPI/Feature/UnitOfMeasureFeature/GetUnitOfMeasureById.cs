using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.UnitOfMeasure.DTOs;
using BS.Services.UnitOfMeasure;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;

namespace CSAPI.Feature.UnitOfMeasureFeature
{
    public class GetUnitOfMeasureById : IUnitOfMeasureFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
           .MapGet($"/{nameof(GetUnitOfMeasureById)}", Handle)
           .WithSummary("Get Unit of Measure By ID")
           .Produces(200)
           .Produces(404)
           .Produces(500)
           .Produces<List<ResponseUnitOfMeasure>>();

        private static async Task<IResult> Handle(string measureId, ICustomLogger logger, IUnitOfMeasureService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetUnitOfMeasureById(measureId,ct);
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
