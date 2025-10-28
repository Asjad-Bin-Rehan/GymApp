using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.InspectionCardService.DTOs;
using CSAPI.Extensions.RouteHandler;
using BS.Services.InspectionCardService;

namespace CSAPI.Feature.InspectionCardFeature
{
    public class AddInspectionCardWithCharacteristics : IInspectionCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapPost($"/{nameof(AddInspectionCardWithCharacteristics)}", Handle)
                           .WithSummary("Add an Inspection Card with Characteristics to DB")
                           .Produces(201)
                           .Produces(402)
                           .Produces(500)
                           .WithRequestValidation<AddInspectionCardWithCharacteristicsDTO>()
                           .Produces<bool>();

        public class RequestValidator : AbstractValidator<AddInspectionCardWithCharacteristicsDTO>
        {
            IInspectionCardService _svc;
            public RequestValidator(IInspectionCardService svc)
            {
                _svc = svc;

                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Description cannot be only whitespaces.")
                    .Must(x => x?.Trim() == x).WithMessage("Description can not contain leading or trailing spaces.")
                    .MustAsync(IsNotExists).WithMessage("Description cannot be duplicate.")
                    ;

                RuleFor(x => x.CharacteristicsIds)
                    .NotNull().WithMessage("CharacteristicsIds are required.")
                    .NotEmpty().WithMessage("CharacteristicsIds cannot be empty.")
                    .ForEach(id => id.NotEmpty().WithMessage("Each CharacteristicId must be a non-empty string.")
                    .Must(x => x.Trim() == x).WithMessage("Each CharacteristicId should not contain leading or trailing spaces."))
                    ;
            }

            #region Custom FluentValidations
            async Task<bool> IsNotExists(string? description, CancellationToken ct)
            {
                return !await _svc.IsDescriptionExists(description, ct);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] AddInspectionCardWithCharacteristicsDTO req, IUserContext user, ICustomLogger logger, IInspectionCardService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddInspectionCardWithCharacteristics(req, user.Data.UserId ?? "Anonymous", ct);
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
