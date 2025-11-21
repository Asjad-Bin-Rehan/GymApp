using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQACavityService;
using BS.Services.ProductionQACavityService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Extensions.RouteHandler;

namespace CSAPI.Feature.ProductionQACavityFeature
{
    public class UpdateProductionQACavity : IProductionQACavityFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdateProductionQACavity)}", Handle)
            .WithSummary("Update Production QA Cavity")
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<UpdateProductionQACavityDTO>()
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<UpdateProductionQACavityDTO>
        {
            IProductionQACavityService _svc;
            public RequestValidator(IProductionQACavityService svc)
            {
                _svc = svc;
                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("ID is required.")
                    .Must(IsUnused).WithMessage("ID is already in use, cannot update.")
                    .Must(x => x.Trim() == x).WithMessage("ID cannot contain leading or trailing spaces.");
            }

            private bool IsUnused(string id)
            {
                return true;
            }
        }

        private static async Task<IResult> Handle([FromBody] UpdateProductionQACavityDTO req, IUserContext user, ICustomLogger logger, IProductionQACavityService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.UpdateProductionQACavity(req, user.Data.UserId ?? "Anonymous", ct);
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
