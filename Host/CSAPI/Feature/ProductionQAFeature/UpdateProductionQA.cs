using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQAService;
using BS.Services.ProductionQAService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ProductionQAFeature
{
    public class UpdateProductionQA : IProductionQAFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdateProductionQA)}", Handle)
            .WithSummary("Update or Soft-Delete Production QA from DB")
            .Produces(200)
            .Produces(400)
            .Produces(402)
            .Produces(500)
            .WithRequestValidation<UpdateProductionQADTO>()
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<UpdateProductionQADTO>
        {
            IProductionQAService _svc;
            public RequestValidator(IProductionQAService svc)
            {
                _svc = svc;

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("ID is required.")
                    .Must(x => x.Trim() == x).WithMessage("ID cannot contain leading or trailing spaces.");
            }
        }

        private static async Task<IResult> Handle([FromBody] UpdateProductionQADTO req, IUserContext user, ICustomLogger logger, IProductionQAService svc, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.UpdateProductionQA(req, user.Data.UserId ?? "Anonymous", cancellationToken);
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
