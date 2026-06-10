namespace BS.Services.SlotService.DTOs
{
    public class UpsertSlotRequest
    {
        public List<UpsertSlotObject> Slots { get; set; } = [];
    }

    public class UpsertSlotObject
    {
        public string? Id { get; set; }
        public string? Day { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool? IsAvailable { get; set; }
        public float? CalculatedPrice { get; set; }

        // FKs
        public string? CourtId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpsertSlotResponse
    {
        public List<string> Ids { get; set; } = [];
    }
}
