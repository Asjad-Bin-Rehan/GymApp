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
    public class AddTestRecords : INextIntCodeFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddTestRecords)}", Handle)
            .WithSummary("Add Test Records to DB")
            .Produces(201)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<RequestAddTestRecordsValidator>()
            .Produces<bool>();

        public class RequestAddTestRecordsValidator : AbstractValidator<AddTestRecordsDTO>
        {
            public RequestAddTestRecordsValidator()
            {
                //RuleFor(x => x.EntityName).NotEmpty().WithMessage("EntityName is required.");
            }
        }

        private static async Task<IResult> Handle([FromBody] AddTestRecordsDTO req, IUserContext user, ICustomLogger logger, INextIntCodeService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddTestRecords(req, user.Data.UserId ?? "Anonymous", ct);
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
