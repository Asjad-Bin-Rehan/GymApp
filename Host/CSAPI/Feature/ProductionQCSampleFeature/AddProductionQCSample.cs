using BS.Services.ProductionQCSampleService.DTOs;
using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.ProductionQCSampleService;
using CSAPI.Extensions.RouteHandler;
using FluentValidation;

namespace CSAPI.Feature.ProductionQCSampleFeature
{
    public class AddProductionQCSample : IProductionQCSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddProductionQCSample)}", Handle)
            .WithSummary("Add a Production QC Sample")
            .Produces(201)
            .Produces(500)
            .WithRequestValidation<AddProductionQCSampleDTO>()
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<AddProductionQCSampleDTO>
        {
            IProductionQCSampleService _svc;
            public RequestValidator(IProductionQCSampleService svc)
            {
                _svc = svc;

                RuleFor(x => x.QcId).NotEmpty().WithMessage("Qx ID is required")
                    .MustAsync(IsQxIdAvailable).WithMessage("Qx ID not found.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Qx ID cannot be only whitespaces.")
                    .Must(x => x?.Trim() == x).WithMessage("Qx ID can not contain leading or trailing spaces.")
                    ;

                RuleFor(x => x.InspectionObjects)
                    .NotEmpty().WithMessage("Inspection object can not be empty.");
            }
            #region Custom FluentValidations
            async Task<bool> IsQxIdAvailable(string? Id, CancellationToken ct)
            {
                return await _svc.IsProductionQcIdAvailable(Id, ct);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] AddProductionQCSampleDTO req, IUserContext user, ICustomLogger logger, IProductionQCSampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddProductionQCSample(req, user.Data.UserId ?? "Anonymous", ct);
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
