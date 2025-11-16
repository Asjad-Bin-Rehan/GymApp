using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class UpdatePartnerGym : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdatePartnerGym)}", Handle)
            .WithSummary("Update partner gym")
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle([FromBody] UpdatePartnerGymDTO request, [FromServices] IPartnerGymService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                await svc.UpdatePartnerGymRaw(request, ct);
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
