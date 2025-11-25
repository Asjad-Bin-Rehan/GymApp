using BS.Services.PartnerGymService;
using CSAPI.Common;
using CSAPI.Feature.PartnerGym;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace BS.Features.PartnerGymFeature
{
    public class GetPartnerGymsCount : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("/PartnerGyms/Count", Handle)
               .WithName("GetPartnerGymsCount")
               .WithSummary("Returns total number of partner gyms registered in system");
        }

        private static async Task<IResult> Handle(
            [FromServices] IPartnerGymService partnerGymService,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                // 1️⃣ Validate DI service
                if (partnerGymService == null)
                {
                    return ApiResponseHelper.Convert(false, false,
                        "Partner gym service not available", 500, null);
                }

                // 2️⃣ Fetch count
                var countDto = await partnerGymService.GetPartnerGymsCountRaw(ct);

                // 3️⃣ Validate result
                if (countDto == null)
                {
                    return ApiResponseHelper.Convert(false, false,
                        "Failed to fetch partner gyms count", 500, null);
                }

                if (countDto.total_gyms < 0)
                {
                    return ApiResponseHelper.Convert(false, false,
                        "Invalid partner gyms count value returned", 500, null);
                }

                // 4️⃣ Success response
                return ApiResponseHelper.Convert(true, true,
                    "Partner gyms count fetched successfully", 200, countDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                return ApiResponseHelper.Convert(false, false,
                    "Something went wrong", 500, null);
            }
        }
    }
}
