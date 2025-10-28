using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.InspectionAttributeService;
using BS.Services.InspectionAttributeService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.InspectionAttributeFeature
{
    public class GetInspectionAttributeById : IInspectionAttributeFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetInspectionAttributeById)}", Handle)
            .WithSummary("Get an inspection attribute by ID")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponseInspectionAttribute>();

        private static async Task<IResult> Handle([FromQuery] string id, IInspectionAttributeService svc, ICustomLogger logger, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.GetInspectionAttributeById(id, ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
