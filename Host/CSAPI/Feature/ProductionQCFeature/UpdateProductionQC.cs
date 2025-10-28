using BS.CustomExceptions.CustomExceptionMessage;
using CSAPI.Common;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;
using CSAPI.Extensions.RouteHandler;
using BS.Services.ProductionQCService;
using BS.Services.ProductionQCService.DTOs;

namespace CSAPI.Feature.ProductionQCFeature
{
    public class UpdateProductionQC : IProductionQCFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapPut($"/{nameof(UpdateProductionQC)}", Handle)
                           .WithSummary("Update or Soft-Delete Production QC from DB")
                           .Produces(200)
                           .Produces(400)
                           .Produces(402)
                           .Produces(500)
                           .WithRequestValidation<UpdateProductionQCDTO>()
                           .Produces<bool>();

        public class RequestValidator : AbstractValidator<UpdateProductionQCDTO>
        {
            IProductionQCService _svc;
            public RequestValidator(IProductionQCService svc)
            {
                _svc = svc;

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("ID is required.")
                    .Must(IsUnUsed).WithMessage("Already being used so can't update.")
                    .Must(x => x.Trim() == x).WithMessage("ID can not contain leading or trailing spaces.")
                    ;

            }

            #region Custom FluentValidations
            public bool IsUnUsed(string Id)
            {
                return true; //!_svc.IsUoMUsed(Id);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] UpdateProductionQCDTO req, IUserContext user, ICustomLogger logger, IProductionQCService svc, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await svc.UpdateProductionQC(req, user.Data.UserId ?? "Anonymous", cancellationToken);
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