using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ItemInspectionCardService;
using BS.Services.ItemInspectionCardService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ItemInspectionCardFeature;

public class UpdateItemInspectionCard : IItemInspectionCardFeature
{
    public static void Map(IEndpointRouteBuilder app) => app
                           .MapPut($"/{nameof(UpdateItemInspectionCard)}", Handle)
                           .WithSummary("Update Item Inspection Card in DB")
                           .Produces(200)
                           .Produces(400)
                           .Produces(402)
                           .Produces(500)
                           .WithRequestValidation<UpdateItemInspectionCardDTO>()
                           .Produces<bool>();

    public class RequestValidator : AbstractValidator<UpdateItemInspectionCardDTO>
    {
        IItemInspectionCardService _svc;
        public RequestValidator(IItemInspectionCardService svc)
        {
            _svc = svc;

            RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("ID is required.")
                    .MustAsync(IsUnUsed).WithMessage("Already being used so can't update.")
                    .Must(x => x.Trim() == x).WithMessage("ID can not contain leading or trailing spaces.")
                    ;

            //RuleFor(x => x.ItemDescription)
            //    .NotEmpty().WithMessage("ItemDescription is required.")
            //    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("ItemDescription cannot be only whitespaces.");

        }

        #region Custom FluentValidations
        async Task<bool> IsUnUsed(string Id, CancellationToken ct)
        {
            return !await _svc.IsItemInspectionCardUsed(Id, ct);
        }
        #endregion CustomFluentValidations
    }

    private static async Task<IResult> Handle([FromBody] UpdateItemInspectionCardDTO req, IUserContext user, ICustomLogger logger, IItemInspectionCardService svc, CancellationToken cancellationToken)
    {
        int statusCode = HTTPStatusCode200.Ok;
        string message = "Success";
        try
        {
            var result = await svc.UpdateItemInspectionCard(req, user.Data.UserId ?? "Anonymous", cancellationToken);
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