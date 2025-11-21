using BS.Services.ProductionQCSampleService.DTOs;
using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.ProductionQCSampleService;
using CSAPI.Extensions.RouteHandler;

namespace CSAPI.Feature.ProductionQCSampleFeature
{
    public class UpdateProductionQCSample : IProductionQCSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdateProductionQCSample)}", Handle)
            .WithSummary("Update Production QC Sample")
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<UpdateProductionQCSampleDTO>()
            .Produces<bool>();

        private static async Task<IResult> Handle([FromBody] UpdateProductionQCSampleDTO req, IUserContext user, ICustomLogger logger, IProductionQCSampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.UpdateProductionQCSample(req, user.Data.UserId ?? "Anonymous", ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                return ApiResponseHelper.Convert(true, false, message, statusCode, false);
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
