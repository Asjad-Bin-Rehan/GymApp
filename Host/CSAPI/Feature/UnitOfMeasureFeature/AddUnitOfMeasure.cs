using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.UnitOfMeasure;
using BS.Services.UnitOfMeasure.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.UnitOfMeasureFeature
{
    public class AddUnitOfMeasure : IUnitOfMeasureFeature
    {
            public static void Map(IEndpointRouteBuilder app) => app
                           .MapPost($"/{nameof(AddUnitOfMeasure)}", Handle)
                           .WithSummary("Add a Unit Of Measure to DB")
                           .Produces(201)
                           .Produces(402)
                           .Produces(500)
                           .WithRequestValidation<AddUnitOfMeasureDTO>()
                           .Produces<string>();

            public class RequestValidator : AbstractValidator<AddUnitOfMeasureDTO>
            {
                IUnitOfMeasureService _svc;
                public RequestValidator(IUnitOfMeasureService svc)
                {
                    _svc = svc;

                    RuleFor(x => x.Description)
                        .NotEmpty().WithMessage("Description is required.")
                        .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Description cannot be only whitespaces.")
                        .MustAsync(IsNotExists).WithMessage("Description cannot be duplicate.")
                        .Must(x => x?.Trim() == x).WithMessage("Description can not contain leading or trailing spaces.")
                        ;
                    
                    RuleFor(x => x.UoMCode)
                        .NotEmpty().WithMessage("UoMCode is required.")
                        .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("UoMCode cannot be only whitespaces.")
                        .MustAsync(IsNotExists).WithMessage("UoMCode cannot be duplicate.")
                        .Must(x => x?.Trim() == x).WithMessage("UoMCode can not contain leading or trailing spaces.")
                        ;
                }
                #region Custom FluentValidations
                async Task<bool> IsNotExists(string? Id, CancellationToken ct)
                {
                    return !await _svc.IsUoMNameOrCodeExists(Id, ct);
                }
                #endregion Custom FluentValidations
            }

        private static async Task<IResult> Handle([FromBody] AddUnitOfMeasureDTO req, IUserContext user, ICustomLogger logger, IUnitOfMeasureService svc, CancellationToken cancellationToken)
            {
                int statusCode = HTTPStatusCode200.Created;
                string message = "Success";
                try
                {
                    var result = await svc.AddUnitOfMeasure(req, user.Data.UserId ?? "Anonymous", cancellationToken);
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
