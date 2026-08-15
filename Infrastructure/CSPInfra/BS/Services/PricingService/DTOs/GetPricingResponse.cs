using Helpers.CommonModels;

namespace BS.Services.PricingService.DTOs
{
    public class GetPricingResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }
        public string? Day { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public float? Price { get; set; }

        // FKs
        public string? CourtId { get; set; }
    }
}
