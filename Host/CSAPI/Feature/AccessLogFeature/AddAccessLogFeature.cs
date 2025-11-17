using BS.Services.AccessLogService;
using BS.Services.AccessLogService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.AccessLogFeature
{
    public class AddAccessLog : IAccessLogFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddAccessLog)}", Handle)
            .WithSummary("Add a new access log")
            .Produces<bool>()
            .Produces(200)
            .Produces(400)
            .Produces(500)
            .WithRequestValidation<AddAccessLogDTO>();

        public class RequestValidator : AbstractValidator<AddAccessLogDTO>
        {
            public RequestValidator()
            {
                RuleFor(x => x.user_id).NotNull().WithMessage("user_id is required");
                RuleFor(x => x.gym_id).NotNull().WithMessage("gym_id is required");
            }
        }

        private static async Task<IResult> Handle([FromBody] AddAccessLogDTO request, IAccessLogService svc, ICustomLogger logger, CancellationToken ct)
        {
            int statusCode = 200;
            string message = "Success";

            try
            {
                var result = await svc.AddAccessLogRaw(request, ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
