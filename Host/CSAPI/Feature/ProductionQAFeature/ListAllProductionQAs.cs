using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQAService;
using BS.Services.ProductionQAService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Helpers.CustomExceptionThrower;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ProductionQAFeature
{
    public class ListAllProductionQAs : IProductionQAFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllProductionQAs)}", Handle)
            .WithSummary("List all Production QA Records with Item Details by optional PageSize & PageNumber")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ListProductionQAResponse>();

        private static async Task<IResult> Handle([FromQuery] bool? needTotalCount, [FromQuery] int? pageSize, [FromQuery] int? pageNumber, ICustomLogger logger, IProductionQAService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.ListAllProductionQAs(pageSize ?? int.MaxValue, pageNumber ?? 1, ct);
                result.TotalRecords = (needTotalCount!=false) ? await svc.GetTotalRecords(ct) : result.TotalRecords;
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentFalseException e)
            {
                var result = new ListProductionQAResponse()
                {
                    PageSize = pageSize ?? int.MaxValue,
                    PageNumber = pageNumber ?? int.MaxValue,
                    TotalRecords = (needTotalCount!=false) ? await svc.GetTotalRecords(ct) : 0
                };
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                return ApiResponseHelper.Convert(true, false, message, statusCode, result);
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
