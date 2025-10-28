namespace BS.Services.InspectionCardService.DTOs
{
    public class UpdateInspectionCardDTO
    {
        public string Id { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string> CharacteristicsIds { get; set; } = [];
        public List<string> DetachInspectionCharacteristicIds { get; set; } = [];
    }
}
