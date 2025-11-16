using BS.Services.LocationService;
using BS.Services.LocationService.DTOs;
using CSAPI.Common;
using CSAPI.Extensions.RouteHandler;
using CustomHTTP;
using FluentValidation;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.LocationFeature
{
    public class AddLocation : ILocationFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddLocation)}", Handle)
            .WithSummary("Add new location")
            .Produces<int>()
            .WithRequestValidation<AddLocationDTO>();

        public class RequestValidator : AbstractValidator<AddLocationDTO>
        {
            public RequestValidator()
            {
                RuleFor(x => x.city).NotEmpty();
                RuleFor(x => x.country).NotEmpty();
            }
        }

        private static async Task<IResult> Handle([FromBody] AddLocationDTO request, ILocationService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var id = await svc.AddLocationRaw(request, ct);
                return ApiResponseHelper.Convert(true, true, "Location added", 200, id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
