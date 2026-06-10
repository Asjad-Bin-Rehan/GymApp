using Helpers.CommonModels;

namespace BS.Services.CustomerProfileService.DTOs
{
    public class GetCustomerProfileResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }

        // FKs
        public string? UserId { get; set; }
    }
}
