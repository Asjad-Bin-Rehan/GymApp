using Helpers.CommonModels;

namespace BS.Services.VenueService.DTOs
{
    public class GetVenueResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? MapUrl { get; set; }
        public string? DpUrl { get; set; }
        public string? Sports { get; set; }

        // FKs
        public string? OrganizationId { get; set; }
        public string? LocationId { get; set; }
    }
}
