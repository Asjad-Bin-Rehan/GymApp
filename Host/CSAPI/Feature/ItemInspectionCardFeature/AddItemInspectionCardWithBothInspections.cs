using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ItemInspectionCardService.DTOs;
using BS.Services.ItemInspectionCardService;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Extensions.RouteHandler;

namespace CSAPI.Feature.ItemInspectionCardFeature
{
    public class AddItemInspectionCardWithBothInspections : IItemInspectionCardFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapPost($"/{nameof(AddItemInspectionCardWithBothInspections)}", Handle)
                           .WithSummary("Add an Item Inspection Card with both Qualitative and Quantitative Inspections")
                           .Produces(201)
                           .Produces(402)
                           .Produces(500)
                           .WithRequestValidation<AddItemInspectionCardWithBothInspectionsDTO>()
                           .Produces<string>();

        public class RequestValidator : AbstractValidator<AddItemInspectionCardWithBothInspectionsDTO>
        {
            IItemInspectionCardService _svc;
            public RequestValidator(IItemInspectionCardService svc)
            {
                _svc = svc;

                RuleFor(x => x.ItemCode).NotEmpty().WithMessage("ItemCode is required.");

                RuleFor(x => x.ItemName).NotEmpty().WithMessage("ItemName is required.");

                RuleForEach(x => x.QualitativeInspectionObjects)
                    .Must(x => !string.IsNullOrWhiteSpace(x.InspectionCharacteristicId)).WithMessage("Inspection Characteristic ID can not be null.")
                    ;

                RuleForEach(x => x.QuantitativeInspectionObjects)
                    .Must(x => !string.IsNullOrWhiteSpace(x.InspectionCharacteristicId)).WithMessage("Inspection Characteristic ID can not be null.")
                    ;
            }

            //#region Custom FluentValidations
            //bool IsNotExist(string? ItemCode)
            //{
            //    return !_svc.IsItemExists(ItemCode);
            //}
            //#endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] AddItemInspectionCardWithBothInspectionsDTO req, IUserContext user, ICustomLogger logger, IItemInspectionCardService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddItemInspectionCardWithBothInspections(req, user.Data.UserId ?? "Anonymous", ct);
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
