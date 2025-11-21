namespace BS.Services.InspectionCharactersticService.DTOs
{
    public class AddCharacteristicWithCriteriaDTO
    {
        public string? AttributeId { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? SingleCriteria { get; set; }
        public string? QuantitativeCriteria { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
