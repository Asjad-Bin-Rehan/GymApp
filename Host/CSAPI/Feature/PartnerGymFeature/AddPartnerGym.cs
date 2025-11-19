using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CSAPI.Feature.PartnerGym
{
    public class AddPartnerGym : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddPartnerGym)}", Handle)
            .WithSummary("Add a new partner gym")
            .Produces(200)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromBody] AddPartnerGymDTO request, 
            [FromServices] IPartnerGymService svc, 
            ICustomLogger logger, 
            HttpContext http, 
            CancellationToken ct)
        {
            try
            {
                // Extract admin_id from JWT claims
                var adminIdClaim = http.User.FindFirst("admin_id")?.Value;
                if (adminIdClaim == null)
                    return ApiResponseHelper.Convert(false, false, "Unauthorized", 401, null);

                if (!int.TryParse(adminIdClaim, out int adminId))
                    return ApiResponseHelper.Convert(false, false, "Invalid admin ID", 401, null);

                // Call the new service method
                var gymId = await svc.AddPartnerGymRaw(request, adminId, ct);

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
