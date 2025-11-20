using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class ListActivePartnerGyms : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListActivePartnerGyms)}", Handle)
            .WithSummary("List all active partner gyms")
            .Produces<List<ActiveGymDTO>>()   // uses the new DTO
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromQuery] int limit,
            [FromQuery] int offset,
            [FromServices] IPartnerGymService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var gyms = await svc.ListActivePartnerGymsRaw(limit, offset, ct);
                return ApiResponseHelper.Convert(true, true, "Success", 200, gyms);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
