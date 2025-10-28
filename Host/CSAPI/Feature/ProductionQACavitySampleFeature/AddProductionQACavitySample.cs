using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ProductionQACavitySampleService.DTOs;
using BS.Services.ProductionQACavitySampleService;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Helpers.Auth.Middlewares;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.ProductionQACavitySampleFeature
{
    public class AddProductionQACavitySample : IProductionQACavitySampleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddProductionQACavitySample)}", Handle)
            .WithSummary("Add a Production QA Cavity Sample")
            .Produces(201)
            .Produces(500)
            .WithRequestValidation<AddProductionQACavitySampleDTO>()
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<AddProductionQACavitySampleDTO>
        {
            IProductionQACavitySampleService _svc;
            public RequestValidator(IProductionQACavitySampleService svc)
            {
                _svc = svc;

                RuleFor(x => x.CavityId).NotEmpty().WithMessage("Cavity ID is required")
                    .MustAsync(IsCavityIdAvailable).WithMessage("Cavity ID not found.")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cavity ID cannot be only whitespaces.")
                    .Must(x => x?.Trim() == x).WithMessage("Cavity ID can not contain leading or trailing spaces.")
                    ;

                RuleFor(x => x.InspectionObjects).NotEmpty().WithMessage("Inspection object can not be empty.");
            }
            #region Custom FluentValidations
            async Task<bool> IsCavityIdAvailable(string? Id, CancellationToken ct)
            {
                return await _svc.IsCavityIdAvailable(Id, ct);
            }
            #endregion Custom FluentValidations
        }

        private static async Task<IResult> Handle([FromBody] AddProductionQACavitySampleDTO req, IUserContext user, ICustomLogger logger, IProductionQACavitySampleService svc, CancellationToken ct)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await svc.AddProductionQACavitySample(req, user.Data.UserId ?? "Anonymous", ct);
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
