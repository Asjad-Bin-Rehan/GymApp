using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.InspectionCharactersticService;
using BS.Services.InspectionCharactersticService.DTOs;
using CSAPI.Common;
using CSAPI.Common.Constant;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.InspectionCharacteristicFeature
{
    public class AddCharacteristicWithCriteria : IInspectionCharacteristicFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapPost($"/{nameof(AddCharacteristicWithCriteria)}", Handle)
                           .WithSummary("Add an Inspection Characteristic with Qualitative Criteria to DB")
                           .Produces(201)
                           .Produces(402)
                           .Produces(500)
                           .WithRequestValidation<AddCharacteristicWithCriteriaDTO>()
                           .Produces<string>();

        public class RequestValidator : AbstractValidator<AddCharacteristicWithCriteriaDTO>
        {
            IInspectionCharacteristicService _svc;
            public RequestValidator(IInspectionCharacteristicService svc)
            {
                _svc = svc;

                RuleFor(x => x).MustAsync(IsNotExist).WithMessage("Description, Attribute and Type cannot be duplicate.");

                RuleFor(x => x.AttributeId)
                    .NotEmpty().WithMessage("Attribute ID is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Attribute ID cannot be only whitespaces.")
                    .MustAsync(IsAttributeIdAvailable).WithMessage("Attribute ID not found.")
                    .Must(x => x?.Trim() == x).WithMessage("Attribute ID can not contain leading or trailing spaces.")
                    ;

                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Description cannot be only whitespaces.")
                    .Must(x => x?.Trim() == x).WithMessage("Description can not contain leading or trailing spaces.")
                    ;

                RuleFor(x => x.Type)
                    .NotEmpty().WithMessage("Type is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Type cannot be only whitespaces.")
                    .Must(x => x?.Trim() == x).WithMessage("Type can not contain leading or trailing spaces.")
                    .Must(x => x?.ToLower() == KConstantInspectionType.qualitative || x?.ToLower() == KConstantInspectionType.quantitative || x?.ToLower() == KConstantInspectionType.both).WithMessage("Type can be only Qualitative or Quantitative or both.")
                    ;
            }

            #region Custom FluentValidations
            async Task<bool> IsAttributeIdAvailable(string? Id, CancellationToken ct)
            {
                return await _svc.IsAttributeIdAvailable(Id, ct);
            }
            async Task<bool> IsNotExist(AddCharacteristicWithCriteriaDTO req, CancellationToken ct)
            {
                return !await _svc.IsCharacteristicDescriptionAndAttributeNotExists(req, ct);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] AddCharacteristicWithCriteriaDTO req, IUserContext user, ICustomLogger logger, IInspectionCharacteristicService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddCharacteristicWithCriteria(req, user.Data.UserId ?? "Anonymous", ct);
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