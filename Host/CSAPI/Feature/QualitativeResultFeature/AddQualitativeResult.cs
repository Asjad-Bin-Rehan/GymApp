using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.QualitativeResultService;
using BS.Services.QualitativeResultService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.QualitativeResultFeature
{
    public class AddQualitativeResult : IQualitativeResultFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddQualitativeResult)}", Handle)
            .WithSummary("Add an Qualitative Result to DB")
            .Produces(201)
            .Produces(402)
            .Produces(500)
            .WithRequestValidation<AddQualitativeResultDTO>()
            .Produces<string>();

        public class RequestValidator : AbstractValidator<AddQualitativeResultDTO>
        {
            IQualitativeResultService _svc;
            public RequestValidator(IQualitativeResultService svc)
            {
                _svc = svc;

                RuleFor(x => x.ResultDescription)
                    .NotEmpty().WithMessage("Description is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Description cannot be only whitespaces.")
                    .MustAsync(IsQualitativeResultDescriptionNotExists).WithMessage("Description cannot be duplicate.")
                    .Must(x => x?.Trim() == x).WithMessage("Description can not contain leading or trailing spaces.")
                    ;
            }
            #region Custom FluentValidations
            async Task<bool> IsQualitativeResultDescriptionNotExists(string? Id, CancellationToken ct)
            {
                return !await _svc.IsQualitativeResultDescriptionExists(Id, ct);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] AddQualitativeResultDTO req, IUserContext user, ICustomLogger logger, IQualitativeResultService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddQualitativeResult(req, user.Data.UserId ?? "Anonymous", ct);
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