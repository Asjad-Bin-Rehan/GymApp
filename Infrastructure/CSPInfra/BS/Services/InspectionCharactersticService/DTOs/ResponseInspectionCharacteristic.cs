using Helpers.CommonModels;

namespace BS.Services.InspectionCharactersticService.DTOs
{
    public class ResponseInspectionCharacteristic : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? AttributeId { get; set; }
        public int IntCode { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? SingleCriteria { get; set; }
        public string? QuantitativeCriteria { get; set; }
        public AttributeInInspectionCharacteristic? Attribute { get; set; }
    }

    public class AttributeInInspectionCharacteristic : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
    }
}