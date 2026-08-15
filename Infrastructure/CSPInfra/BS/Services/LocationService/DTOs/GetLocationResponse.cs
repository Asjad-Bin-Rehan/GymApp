using Helpers.CommonModels;

namespace BS.Services.LocationService.DTOs
{
    public class GetLocationResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }

        // FK
        public string? ParentId { get; set; }
    }
}