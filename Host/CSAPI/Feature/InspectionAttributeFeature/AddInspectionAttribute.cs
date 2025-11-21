using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.InspectionAttributeService;
using BS.Services.InspectionAttributeService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.InspectionAttributeFeature
{
    public class AddInspectionAttribute : IInspectionAttributeFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddInspectionAttribute)}", Handle)
            .WithSummary("Adds a new inspection attribute")
            .Produces<bool>()
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<AddInspectionAttributeDTO>()
            ;

        public class RequestValidator : AbstractValidator<AddInspectionAttributeDTO>
        {
            IInspectionAttributeService _svc;
            public RequestValidator(IInspectionAttributeService svc)
            {
                _svc = svc;

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Name cannot be only whitespaces.")
                    .MustAsync(IsNotExist).WithMessage("Name cannot be duplicate.")
                    .Must(x => x?.Trim() == x).WithMessage("Name can not contain leading or trailing spaces.")
                    ;
            }

            #region Custom FluentValidations
            async Task<bool> IsNotExist(string? name, CancellationToken ct)
            {
                return !await _svc.IsInspectionAttributeNameExists(name, ct);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] AddInspectionAttributeDTO request, IUserContext user, IInspectionAttributeService svc, ICustomLogger logger, CancellationToken ct)
        {
            var statusCode = HTTPStatusCode200.Created;
            var message = "Success";
            try
            {
                var result = await svc.AddInspectionAttributeRaw(request, user.Data.UserId ?? "Anonymous", ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (Exception ex)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
