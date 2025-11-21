using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ItemCardService.DTOs;
using BS.Services.ItemCardService;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ItemCardFeature
{
    public class AddItemCard : IItemCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapPost($"/{nameof(AddItemCard)}", Handle)
                           .WithSummary("Just for manual testing - DO NOT INTEGRATE!")
                           .Produces(201)
                           .Produces(402)
                           .Produces(500)
                           .WithRequestValidation<AddItemCardDTO>()
                           .Produces<bool>();
        public class RequestValidator : AbstractValidator<AddItemCardDTO>
        {
            public RequestValidator()
            {
                RuleFor(x => x.ItemCode)
                    .NotEmpty().WithMessage("Item Code is required.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Item Code cannot be only whitespaces.")
                    .Must(x => x?.Trim() == x).WithMessage("ItemCode can not contain leading or trailing spaces.")
                    ;
            }
        }
        private static async Task<IResult> Handle([FromBody] AddItemCardDTO req, IUserContext user, ICustomLogger logger, IItemCardService svc, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddItem(req, user.Data.UserId ?? "Anonymous", cancellationToken);
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
