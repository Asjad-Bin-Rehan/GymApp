using Helpers.CommonModels;

namespace BS.Services.CourtService.DTOs
{
    public class GetCourtResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? SurfaceType { get; set; }

        // FKs
        public string? VenueId { get; set; }
    }
}
