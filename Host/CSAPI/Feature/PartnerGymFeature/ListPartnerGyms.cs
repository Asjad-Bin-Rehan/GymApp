using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class ListPartnerGyms : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListPartnerGyms)}", Handle)
            .WithSummary("List all partner gyms")
            .Produces<List<ResponsePartnerGymDTO>>()
            .Produces(500);

        private static async Task<IResult> Handle([FromQuery] int limit, [FromQuery] int offset, [FromServices] IPartnerGymService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var gyms = await svc.ListPartnerGymsRaw(limit, offset, ct);
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
