using BS.Services.PartnerGymService;
using BS.Services.PartnerGymService.DTOs;
using CSAPI.Common;
using CustomHTTP;
using Logger;
using Microsoft.AspNetCore.Mvc;

namespace CSAPI.Feature.PartnerGym
{
    public class SearchPartnerGyms : IPartnerGymFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(SearchPartnerGyms)}", Handle)
            .WithSummary("Search partner gyms by multiple optional filters")
            .Produces<List<ResponsePartnerGymDTO>>()
            .Produces(400)
            .Produces(404)
            .Produces(500);

        private static async Task<IResult> Handle(
            [FromQuery] int? gym_id,
            [FromQuery] string? name,
            [FromQuery] string? city,
            [FromQuery] string? state,
            [FromQuery] string? country,
            [FromServices] IPartnerGymService svc,
            ICustomLogger logger,
            CancellationToken ct)
        {
            try
            {
                // ❌ Validate at least one filter is provided
                if (!gym_id.HasValue &&
                    string.IsNullOrWhiteSpace(name) &&
                    string.IsNullOrWhiteSpace(city) &&
                    string.IsNullOrWhiteSpace(state) &&
                    string.IsNullOrWhiteSpace(country))
                {
                    return ApiResponseHelper.Convert(
                        false,  // isApiHandled
                        false,  // isRequestSuccess
                        "At least one search filter is required", // message
                        400,    // statusCode
                        null    // data
                    );
                }

                var req = new SearchPartnerGymRequestDTO
                {
                    gym_id = gym_id,
                    name = name,
                    city = city,
                    state = state,
                    country = country
                };

                var rawGyms = await svc.SearchPartnerGymRaw(req, ct);

                // ❌ No gyms found
                if (rawGyms == null || rawGyms.Count == 0)
                {
                    return ApiResponseHelper.Convert(
                        false,
                        false,
                        "No partner gym found for the given filters",
                        404,
                        null
                    );
                }

                // Map RawPartnerGymDTO -> ResponsePartnerGymDTO
                var responseGyms = rawGyms.Select(g => new ResponsePartnerGymDTO
                {
                    gym_id = g.gym_id,
                    gym_name = g.gym_name,
                    location_id = g.location_id,
                    contact_person = g.contact_person,
                    phone = g.phone,
                    partnership_date = g.partnership_date,
                    status = g.status,
                    latitude = g.latitude,
                    longitude = g.longitude
                }).ToList();

                // ✔️ Success response
                return ApiResponseHelper.Convert(
                    true,
                    true,
                    "Success",
                    200,
                    responseGyms
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ApiResponseHelper.Convert(
                    false,
                    false,
                    "Something went wrong",
                    500,
                    null
                );
            }
        }
    }
}
