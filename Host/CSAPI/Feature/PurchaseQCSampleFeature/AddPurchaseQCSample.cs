using BS.Services.PurchaseQCSampleService.DTOs;
using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.PurchaseQCSampleService;
using CSAPI.Extensions.RouteHandler;
using FluentValidation;

namespace CSAPI.Feature.PurchaseQCSampleFeature
{
    public class AddPurchaseQCSample : IPurchaseQCSampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddPurchaseQCSample)}", Handle)
            .WithSummary("Add a Purchase QC Sample")
            .Produces(201)
            .Produces(500)
            .WithRequestValidation<AddPurchaseQCSampleDTO>()
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<AddPurchaseQCSampleDTO>
        {
            IPurchaseQCSampleService _svc;
            public RequestValidator(IPurchaseQCSampleService svc)
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
                return await _svc.IsPurchaseQcIdAvailable(Id, ct);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] AddPurchaseQCSampleDTO req, IUserContext user, ICustomLogger logger, IPurchaseQCSampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddPurchaseQCSample(req, user.Data.UserId ?? "Anonymous", ct);
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
