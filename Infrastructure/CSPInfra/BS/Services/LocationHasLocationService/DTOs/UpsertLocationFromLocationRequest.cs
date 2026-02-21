namespace BS.Services.LocationHasLocationService.DTOs
{
    public class UpsertLocationFromLocationRequest
    {
        public List<UpsertLocationFromLocationObject> Location_From_Locations { get; set; } = [];
    }

    public class UpsertLocationFromLocationObject
    {
        public string? Id { get; set; }

        // FKs
        public string? LocationId { get; set; }
        public string? OtherLocationId { get; set; }

        // Cols
        public float? Distance { get; set; }
        public int? NearbyRank { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpsertLocationFromLocationResponse
    {
        public List<string> Ids { get; set; } = [];
    }
}