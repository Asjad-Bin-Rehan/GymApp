using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class AddPartnerGym : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddPartnerGym)}", Handle)
            .WithSummary("Add a new partner gym")
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle([FromBody] AddPartnerGymDTO request, [FromServices] IPartnerGymService svc, ICustomLogger logger, CancellationToken ct)
        {
            int statusCode = 200;
            string message = "Success";

            try
            {
                await svc.AddPartnerGymRaw(request, ct);
                return ApiResponseHelper.Convert(true, true, message, statusCode, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
