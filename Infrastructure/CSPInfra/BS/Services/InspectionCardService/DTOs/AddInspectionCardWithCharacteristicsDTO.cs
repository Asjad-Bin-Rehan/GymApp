namespace BS.Services.InspectionCardService.DTOs
{
    public class AddInspectionCardWithCharacteristicsDTO
    {
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string> CharacteristicsIds { get; set; } = [];
    }
}
