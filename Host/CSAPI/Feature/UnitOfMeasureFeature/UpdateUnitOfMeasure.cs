using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.UnitOfMeasure.DTOs;
using CSAPI.Extensions.RouteHandler;
using BS.Services.UnitOfMeasure;

namespace CSAPI.Feature.UnitOfMeasureFeature
{
    public class UpdateUnitOfMeasure : IUnitOfMeasureFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapPut($"/{nameof(UpdateUnitOfMeasure)}", Handle)
                           .WithSummary("Update or Soft-Delete Unused UoM from DB")
                           .Produces(200)
                           .Produces(400)
                           .Produces(402)
                           .Produces(500)
                           .WithRequestValidation<UpdateUnitOfMeasureDTO>()
                           .Produces<bool>();

        public class RequestValidator : AbstractValidator<UpdateUnitOfMeasureDTO>
        {
            IUnitOfMeasureService _svc;
            public RequestValidator(IUnitOfMeasureService svc)
            {
                _svc = svc;

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("ID is required.")
                    .MustAsync(IsUnUsed).WithMessage("Already being used so can't update.")
                    .Must(x => x.Trim() == x).WithMessage("ID can not contain leading or trailing spaces.")
                    ;

                RuleFor(x => x.Description)
                        .NotEmpty().WithMessage("Description is required.")
                        .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Description cannot be only whitespaces.")
                        .Must(x => x?.Trim() == x).WithMessage("Description can not contain leading or trailing spaces.")
                        ;

                RuleFor(x => x.UoMCode)
                        .NotEmpty().WithMessage("UoMCode is required.")
                        .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("UoMCode cannot be only whitespaces.")
                        .Must(x => x?.Trim() == x).WithMessage("UoMCode can not contain leading or trailing spaces.")
                        ;
            }

            #region Custom FluentValidations
            async Task<bool> IsUnUsed(string Id, CancellationToken ct)
            {
                return !await _svc.IsUoMUsed(Id, ct);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] UpdateUnitOfMeasureDTO req, IUserContext user, ICustomLogger logger, IUnitOfMeasureService svc, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.UpdateUnitOfMeasure(req, user.Data.UserId ?? "Anonymous", cancellationToken);
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
