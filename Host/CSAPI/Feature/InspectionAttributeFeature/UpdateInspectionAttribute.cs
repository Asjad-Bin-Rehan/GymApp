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
    public class UpdateInspectionAttribute : IInspectionAttributeFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdateInspectionAttribute)}", Handle)
            .WithSummary("Update an existing inspection attribute")
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<UpdateInspectionAttributeDTO>()
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<UpdateInspectionAttributeDTO>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            }
        }

        private static async Task<IResult> Handle([FromBody] UpdateInspectionAttributeDTO request, IInspectionAttributeService svc, IUserContext user, ICustomLogger logger, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var success = await svc.UpdateInspectionAttribute(request, user.Data.UserId ?? "Anonymous", ct);
                return ApiResponseHelper.Convert(success, success, message, statusCode, success);
            }
            catch (ArgumentException e)
            {
                statusCode = HTTPStatusCode400.PaymentRequiredClient;
                message = e.Message;
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                return ApiResponseHelper.Convert(false, false, message, statusCode, false);
            }
        }
    }
}
