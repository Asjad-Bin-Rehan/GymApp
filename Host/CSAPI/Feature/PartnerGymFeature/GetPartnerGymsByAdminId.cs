using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class GetPartnerGymsByAdminId : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetPartnerGymsByAdminId)}", Handle)
            .WithSummary("Get all partner gyms added by a specific admin")
            .Produces<List<ResponsePartnerGymByAdminDTO>>()
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromQuery] int admin_id,
            [FromServices] IPartnerGymService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var gyms = await svc.GetPartnerGymsByAdminIdRaw(admin_id, ct);

                if (gyms == null || gyms.Count == 0)
                    return ApiResponseHelper.Convert(
                        false,
                        false,
                        "No partner gyms found for this admin",
                        404,
                        null
                    );

                return ApiResponseHelper.Convert(
                    true,
                    true,
                    "Partner gyms retrieved successfully",
                    200,
                    gyms
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(
                    false,
                    false,
                    "An unexpected error occurred while fetching partner gyms",
                    500,
                    null
                );
            }
        }
    }
}
