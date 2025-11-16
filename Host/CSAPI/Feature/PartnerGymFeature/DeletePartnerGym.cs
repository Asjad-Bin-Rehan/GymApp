using BS.Services.PartnerGymService;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class DeletePartnerGym : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapDelete($"/{nameof(DeletePartnerGym)}", Handle)
            .WithSummary("Delete partner gym")
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle([FromQuery] int gym_id, [FromServices] IPartnerGymService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                await svc.DeletePartnerGymRaw(gym_id, ct);
                return ApiResponseHelper.Convert(true, true, "Success", 200, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
