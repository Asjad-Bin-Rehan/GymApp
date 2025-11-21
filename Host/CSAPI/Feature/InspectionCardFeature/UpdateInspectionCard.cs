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
    public class UpdateInspectionCard : IInspectionCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapPut($"/{nameof(UpdateInspectionCard)}", Handle)
                           .WithSummary("Update or Soft-Delete Unused Inspection Card from DB")
                           .Produces(200)
                           .Produces(400)
                           .Produces(402)
                           .Produces(500)
                           .WithRequestValidation<UpdateInspectionCardDTO>()
                           .Produces<bool>();

        public class RequestValidator : AbstractValidator<UpdateInspectionCardDTO>
        {
            IInspectionCardService _svc;
            public RequestValidator(IInspectionCardService svc)
            {
                _svc = svc;

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("ID is required.")
                    .Must(x => x.Trim() == x).WithMessage("ID can not contain leading or trailing spaces.")
                    ;

                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Description cannot be only whitespaces.")
                    .Must(x => x?.Trim() == x).WithMessage("Description can not contain leading or trailing spaces.")
                    ;
            }

            //bool IsUnUsed(string? Id)
            //{
            //    return _svc.IsInspectionCardUsed(Id);
            //}
        }

        private static async Task<IResult> Handle([FromBody] UpdateInspectionCardDTO req, IUserContext user, ICustomLogger logger, IInspectionCardService svc, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.UpdateInspectionCard(req, user.Data.UserId ?? "Anonymous", cancellationToken);
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
