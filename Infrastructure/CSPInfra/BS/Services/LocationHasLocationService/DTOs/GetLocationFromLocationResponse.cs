using Helpers.CommonModels;

namespace BS.Services.LocationHasLocationService.DTOs
{
    public class GetLocationFromLocationResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }

        // FKs
        public string? LocationId { get; set; }
        public string? OtherLocationId { get; set; }

        // Cols
        public float? Distance { get; set; }
        public int? NearbyRank { get; set; }
    }
}