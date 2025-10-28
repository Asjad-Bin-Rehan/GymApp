using Helpers.CommonModels;

namespace BS.Services.ProductionQACavitySampleService.DTOs
{
    public class ResponseProductionQACavitySample : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? CavityId { get; set; }
        public bool? IsSamplePassed { get; set; }

        // UI: Sample Header
        public DateTime InspectionDateTime { get; set; }
        public string? InspectionBy { get; set; }
        public int? InspectionQuantity { get; set; }

        // UI: Inspection Rows
        public List<InspectionInfo> InspectionObjects { get; set; } = [];
    }

    public class InspectionInfo : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? QaCavitySampleId { get; set; }
        public string? QualitativeInspectionMappingId { get; set; }
        public string? QuantitativeInspectionMappingId { get; set; }
        public string? QualitativeResultId { get; set; }
        public bool? IsQualitativeResultPassed { get; set; }
        public double? QuantitativeResult { get; set; }
        public bool? IsQuantitativeResultPassed { get; set; }
        public string? Remarks { get; set; }
    }
}
