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
    public class AddProductionQA : IProductionQAFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddProductionQA)}", Handle)
            .WithSummary("Add a Production QA record to DB")
            .Produces(201)
            .Produces(402)
            .Produces(500)
            .WithRequestValidation<AddProductionQADTO>()
            .Produces<string>();

        public class RequestValidator : AbstractValidator<AddProductionQADTO>
        {
            IProductionQAService _svc;
            public RequestValidator(IProductionQAService svc)
            {
                _svc = svc;

                RuleFor(x => x.ItemId).NotEmpty().WithMessage("Item ID is required")
                    .MustAsync(IsItemIdAvailable).WithMessage("Item ID not found.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Item ID cannot be only whitespaces.")
                    .Must(x => x?.Trim() == x).WithMessage("Item ID can not contain leading or trailing spaces.")
                    ;

                RuleFor(x => x.InspectionQuantity)
                    .Must((dto, inspectionQty) => !inspectionQty.HasValue || !dto.OpenQuantity.HasValue || inspectionQty <= dto.OpenQuantity)
                    .WithMessage("Inspection Quantity cannot be greater than Open Quantity.");

                RuleFor(x => x.ReceiveQuantity)
                    .Must((dto, receiveQty) => !receiveQty.HasValue || !dto.OpenQuantity.HasValue || receiveQty <= dto.OpenQuantity)
                    .WithMessage("Receive Quantity cannot be greater than Open Quantity.");
            }
            #region Custom FluentValidations
            async Task<bool> IsItemIdAvailable(string? Id, CancellationToken ct)
            {
                return await _svc.IsItemIdAvailable(Id, ct);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] AddProductionQADTO req, IUserContext user, ICustomLogger logger, IProductionQAService svc, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddProductionQA(req, user.Data.UserId ?? "Anonymous", cancellationToken);
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
