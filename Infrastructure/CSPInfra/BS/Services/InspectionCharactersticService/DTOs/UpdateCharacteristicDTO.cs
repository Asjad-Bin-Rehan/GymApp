namespace BS.Services.InspectionCharactersticService.DTOs
{
    public class UpdateCharacteristicDTO
    {
        public string Id { get; set; }
        public string? AttributeId { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? SingleCriteria { get; set; }
        public string? QuantitativeCriteria { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
