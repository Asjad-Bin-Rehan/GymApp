using Helpers.CommonModels;

namespace BS.Services.InspectionCardService.DTOs
{
    public class ResponseCardWithCriteriaWithCharacteristics : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int? IntCode { get; set; }
        public string? Description { get; set; }
        public List<CharacteristicWithCriteriaInResponse> ResponseCharacteristicWithCriteria { get; set; } = [];
    }
    public class CharacteristicWithCriteriaInResponse : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? AttributeId { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? SingleCriteria { get; set; }
        public string? QuantitativeCriteria { get; set; }
        public AttributeInCharacteristicResponse? Attribute { get; set; }
        public List<string> QualitativeCriteriaResults { get; set; } = [];  // ABANDONED
    }

    public class AttributeInCharacteristicResponse : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
    }
}
