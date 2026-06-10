namespace BS.Services.CourtService.DTOs
{
    public class UpsertCourtRequest
    {
        public List<UpsertCourtObject> Courts { get; set; } = [];
    }

    public class UpsertCourtObject
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? SurfaceType { get; set; }

        // FKs
        public string? VenueId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpsertCourtResponse
    {
        public List<string> Ids { get; set; } = [];
    }
}
