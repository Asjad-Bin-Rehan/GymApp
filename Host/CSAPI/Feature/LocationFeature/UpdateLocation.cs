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
    public class UpdateLocation : ILocationFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdateLocation)}", Handle)
            .WithSummary("Update an existing location")
            .WithRequestValidation<UpdateLocationDTO>();

        public class RequestValidator : AbstractValidator<UpdateLocationDTO>
        {
            public RequestValidator()
            {
                RuleFor(x => x.location_id).GreaterThan(0);
            }
        }

        private static async Task<IResult> Handle([FromBody] UpdateLocationDTO request, ILocationService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var success = await svc.UpdateLocationRaw(request, ct);
                return ApiResponseHelper.Convert(true, success, "Updated", 200, success);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return ApiResponseHelper.Convert(false, false, "Error", 500, null);
            }
        }
    }
}
