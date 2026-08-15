namespace BS.Services.PricingService.DTOs
{
    public class UpsertPricingRequest
    {
        public List<UpsertPricingObject> Pricings { get; set; } = [];
    }

    public class UpsertPricingObject
    {
        public string? Id { get; set; }
        public string? Day { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public float? Price { get; set; }

        // FKs
        public string? CourtId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpsertPricingResponse
    {
        public List<string> Ids { get; set; } = [];
    }
}
