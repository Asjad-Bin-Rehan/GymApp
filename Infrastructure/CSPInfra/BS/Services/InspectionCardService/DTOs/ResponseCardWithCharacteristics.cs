using Helpers.CommonModels;

namespace BS.Services.InspectionCardService.DTOs
{
    public class ResponseCardWithCharacteristics : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int? IntCode { get; set; }
        public string? Description { get; set; }
        public List<InspectionCharacteristicResults> InspectionCharacteristicResults { get; set; } = [];
    }
    public class InspectionCharacteristicResults : ActivityTrackersInResponse
    {
        public string? Id { get; set; }
        public int? IntCode { get; set; }
        public string? AttributeId { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? SingleCriteria { get; set; }
        public string? QuantitativeCriteria { get; set; }
        public AttributeInCardResponse? Attribute { get; set; }
    }

    public class AttributeInCardResponse : ActivityTrackersInResponse
    {
        public string Id { get; set;} = string.Empty;
        public int IntCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
    }
}
