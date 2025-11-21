using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQACavityService;
using BS.Services.ProductionQACavityService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ProductionQACavityFeature
{
    public class AddProductionQACavity : IProductionQACavityFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddProductionQACavity)}", Handle)
            .WithSummary("Add a Production QA Cavity")
            .Produces(201)
            .Produces(500)
            .WithRequestValidation<AddProductionQACavityDTO>()
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<AddProductionQACavityDTO>
        {
            public RequestValidator()
            {
                
            }
        }

        private static async Task<IResult> Handle([FromBody] AddProductionQACavityDTO req, IUserContext user, ICustomLogger logger, IProductionQACavityService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddProductionQACavity(req, user.Data.UserId ?? "Anonymous", ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
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
