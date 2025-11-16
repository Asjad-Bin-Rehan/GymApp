using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class AddPartnerGymWithLocation : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/CSAPI/IPartnerGymFeature/AddPartnerGym", Handle)
            .WithSummary("Adds a new partner gym along with location")
            .Accepts<AddPartnerGymDTO>("application/json")
            .Produces<int>(200) // returning gym_id
            .Produces(400)
            .Produces(500);

        private static async Task<IResult> Handle([FromBody] AddPartnerGymDTO request, IPartnerGymService svc, ICustomLogger logger, CancellationToken ct)
        {
            try
            {
                var gymId = await svc.AddPartnerGymWithLocation(request, ct);
                return ApiResponseHelper.Convert(true, true, "Partner Gym created successfully", 200, gymId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(false, false, "Something went wrong", 500, null);
            }
        }
    }
}
