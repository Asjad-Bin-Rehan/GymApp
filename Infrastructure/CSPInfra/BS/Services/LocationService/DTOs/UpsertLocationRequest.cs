namespace BS.Services.LocationService.DTOs
{
    public class UpsertLocationRequest
    {
        public List<UpsertLocationObject> Locations { get; set; } = [];
    }

    public class UpsertLocationObject
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string? ParentId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpsertLocationResponse
    {
        public List<string> Ids { get; set; } = [];
    }
}