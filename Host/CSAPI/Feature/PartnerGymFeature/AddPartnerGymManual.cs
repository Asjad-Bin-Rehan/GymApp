using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class AddPartnerGymManual : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddPartnerGymManual)}", Handle)
            .WithSummary("Add a new partner gym manually (frontend provides admin_id)")
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromBody] AddPartnerGymManualDTO request,
            [FromServices] IPartnerGymService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                if (request.admin_id <= 0)
                    return ApiResponseHelper.Convert(false, false, "admin_id is required!", 400, null);

                var gymId = await svc.AddPartnerGymManual(request, ct);

                return ApiResponseHelper.Convert(true, true, "Partner gym added successfully", 200, new { gym_id = gymId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
