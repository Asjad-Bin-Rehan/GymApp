using Helpers.CommonModels;

namespace BS.Services.SlotService.DTOs
{
    public class GetSlotResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }
        public string? Day { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool? IsAvailable { get; set; }
        public float? CalculatedPrice { get; set; }

        // FKs
        public string? CourtId { get; set; }
    }
}
