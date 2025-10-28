using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQACavitySampleService.DTOs;
using BS.Services.ProductionQACavitySampleService;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Extensions.RouteHandler;

namespace CSAPI.Feature.ProductionQACavitySampleFeature
{
    public class UpdateProductionQACavitySample : IProductionQACavitySampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdateProductionQACavitySample)}", Handle)
            .WithSummary("Update Production QA Cavity Sample")
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<UpdateProductionQACavitySampleDTO>()
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<UpdateProductionQACavitySampleDTO>
        {
            public RequestValidator()
            {
                
            }
        }

        private static async Task<IResult> Handle([FromBody] UpdateProductionQACavitySampleDTO req, IUserContext user, ICustomLogger logger, IProductionQACavitySampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.UpdateProductionQACavitySample(req, user.Data.UserId ?? "Anonymous", ct);
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
                logger.LogError(e.Message);
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
