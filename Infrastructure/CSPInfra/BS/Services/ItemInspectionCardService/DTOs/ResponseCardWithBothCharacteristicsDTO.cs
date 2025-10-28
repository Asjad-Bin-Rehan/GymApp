using Helpers.CommonModels;

namespace BS.Services.ItemInspectionCardService.DTOs
{
    public class ResponseCardWithBothCharacteristicsDTO : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? Type { get; set; }
        public int InspectionCardIntCode { get; set; }
        public string? CardDescription { get; set; }
        public string? ItemDescription { get; set; }
        public string? ItemId { get; set; }
        public string? InspectionCardId { get; set; }
        public List<QualitativeInspectionDisplay> QualitativeInspectionObjects { get; set; } = [];
        public List<QuantitativeInspectionDisplay> QuantitativeInspectionResults { get; set; } = [];
    }

    public class QualitativeInspectionDisplay : ActivityTrackersInResponse
    {
        public string? QualitativeInspectionId { get; set; }
        public string? Id { get; set; }                                         // InspectionCharacteristicId
        public string? InspectionCharacteristicName { get; set; }
        public string? InspectionCharacteristicSingleCriteria { get; set; }
        public string? InspectionCharacteristicQuantitativeCriteria { get; set; }
        public string? InspectionCharacterisicMappingId { get; set; }
        public bool IsMandatory { get; set; } = false;
        public bool? IsQcCritical { get; set; }
        public bool? IsQcFloor { get; set; }
        public bool? IsQcLab { get; set; }
        public bool? IsDispatch { get; set; }
        public bool? IsIncoming { get; set; }
        public bool? IsTrial { get; set; }
        public AttributeInIIC? Attribute { get; set; }
        public List<QualitativeResultPassStatusDisplay> QualitativeResultPassStatusResults { get; set; } = [];
        public List<int> QualitativeResultFailStatusResults { get; set; } = []; // ABANDONED BUT KEPT FOR FE
        public string? QualitativeSpec { get; set; }
    }

    public class QualitativeResultPassStatusDisplay : ActivityTrackersInResponse
    {
        public string? PassStatusId { get; set; }
        public string? QualitativeResultId { get; set; }
        public string? ResultDescription { get; set; }
        public bool? IsPassed { get; set; }
    }

    public class QuantitativeInspectionDisplay : ActivityTrackersInResponse
    {
        public string? QuantitativeInspectionId { get; set; }
        public string? Id { get; set; }                                         // InspectionCharacteristicId
        public string? InspectionCharacteristicName { get; set; }
        public string? InspectionCharacterisicMappingId { get; set; }
        public bool IsMandatory { get; set; } = false;
        public string? UoMId { get; set; }
        public string? UoMCode { get; set; }
        public string? UoMName { get; set; }
        public double? Target { get; set; }
        public double? Max { get; set; }
        public double? Min { get; set; }
        public bool? IsQcCritical { get; set; }
        public bool? IsQcFloor { get; set; }
        public bool? IsQcLab { get; set; }
        public bool? IsDispatch { get; set; }
        public bool? IsIncoming { get; set; }
        public bool? IsTrial { get; set; }
        public AttributeInIIC? Attribute { get; set; }
        public string? QuantitativeSpec { get; set; }
        public double? UpperLimit { get; set; }
        public double? LowerLimit { get; set; }
    }

    public class AttributeInIIC : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
    }
}
