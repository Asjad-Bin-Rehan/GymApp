using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.InspectionCharactersticService.DTOs;
using CSAPI.Extensions.RouteHandler;
using BS.Services.InspectionCharactersticService;
using CSAPI.Common.Constant;

namespace CSAPI.Feature.InspectionCharacteristicFeature
{
    public class UpdateCharacteristic : IInspectionCharacteristicFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapPut($"/{nameof(UpdateCharacteristic)}", Handle)
                           .WithSummary("Update or Soft-Delete Unused Inspection Characteristic from DB")
                           .Produces(200)
                           .Produces(400)
                           .Produces(402)
                           .Produces(500)
                           .WithRequestValidation<UpdateCharacteristicDTO>()
                           .Produces<bool>();

        public class RequestValidator : AbstractValidator<UpdateCharacteristicDTO>
        {
            IInspectionCharacteristicService _svc;
            public RequestValidator(IInspectionCharacteristicService svc)
            {
                _svc = svc;

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("ID is required.")
                    .MustAsync(IsUnUsed).WithMessage("Already being used so can't update")
                    .Must(x => x.Trim() == x).WithMessage("ID can not contain leading or trailing spaces.")
                    ;

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
            async Task<bool> IsUnUsed(string? Id, CancellationToken ct)
            {
                return !await _svc.IsCharacteristicUsed(Id, ct);
            }

            async Task<bool> IsAttributeIdAvailable(string? Id, CancellationToken ct)
            {
                return await _svc.IsAttributeIdAvailable(Id, ct);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] UpdateCharacteristicDTO req, IUserContext user, ICustomLogger logger, IInspectionCharacteristicService svc, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.UpdateCharacterstic(req, user.Data.UserId ?? "Anonymous", cancellationToken);
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
