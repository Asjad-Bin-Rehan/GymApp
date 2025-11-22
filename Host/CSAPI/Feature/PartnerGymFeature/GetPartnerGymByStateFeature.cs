using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class GetPartnerGymByState : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetPartnerGymByState)}", Handle)
            .WithSummary("Get partner gyms by state")
            .Produces<List<ResponsePartnerGymDTO>>()
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromQuery] string state,
            [FromServices] IPartnerGymService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                var gyms = await svc.GetPartnerGymByStateRaw(state, ct);
                if (gyms.Count == 0)
                    return ApiResponseHelper.Convert(false, false, "No gyms found", 404, null);

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
