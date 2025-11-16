using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class GetPartnerGymById : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetPartnerGymById)}", Handle)
            .WithSummary("Get partner gym by id")
            .Produces<ResponsePartnerGymDTO>()
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle([FromQuery] int gym_id, [FromServices] IPartnerGymService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var gym = await svc.GetPartnerGymByIdRaw(gym_id, ct);
                if (gym == null)
                    return ApiResponseHelper.Convert(false, false, "Gym not found", 404, null);

                return ApiResponseHelper.Convert(true, true, "Success", 200, gym);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
