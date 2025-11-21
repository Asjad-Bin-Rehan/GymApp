using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using BS.Services.NextIntCodeService;
using BS.Services.NextIntCodeService.DTOs;

namespace CSAPI.Feature.NextIntCodeFeature
{
    public class AddLogPostSap : INextIntCodeFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddLogPostSap)}", Handle)
            .WithSummary("Add POST to SAP log entry")
            .Produces(201)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<RequestValidator>()
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<AddLogPostSapDTO>
        {
            public RequestValidator()
            {
                RuleFor(x => x.QType)
                    .NotEmpty().WithMessage("QType is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("QType cannot be only whitespaces.")
                    ;

                RuleFor(x => x.QId)
                    .NotEmpty().WithMessage("QId is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("QId cannot be only whitespaces.")
                    ;

                RuleFor(x => x.BMR)
                    .NotEmpty().WithMessage("BMR is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("BMR cannot be only whitespaces.")
                    ;

                RuleFor(x => x.QCode)
                    .NotEmpty().WithMessage("QCode is required.")
                    ;
            }
        }

        private static async Task<IResult> Handle([FromBody] AddLogPostSapDTO req, IUserContext user, ICustomLogger logger, INextIntCodeService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";

            try
            {
                var result = await svc.AddLogPostSap(req, user.Data.UserId ?? "Anonymous", ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
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
