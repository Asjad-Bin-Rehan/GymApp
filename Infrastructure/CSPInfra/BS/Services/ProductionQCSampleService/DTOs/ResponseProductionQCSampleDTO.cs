using Helpers.CommonModels;

namespace BS.Services.ProductionQCSampleService.DTOs
{
    public class ResponseProductionQCSample : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? QcId { get; set; }
        public bool? IsSamplePassed { get; set; }

        // UI: Sample Header
        public DateTime InspectionDateTime { get; set; }
        public string? InspectionBy { get; set; }
        public int? InspectionQuantity { get; set; }

        // UI: Inspection Rows
        public List<InspectionResult> InspectionObjects { get; set; } = [];
    }

    public class InspectionResult : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? QcSampleId { get; set; }
        public string? QualitativeInspectionMappingId { get; set; }
        public string? QuantitativeInspectionMappingId { get; set; }
        public string? QualitativeResultId { get; set; }
        public bool? IsQualitativeResultPassed { get; set; }
        public double? QuantitativeResult { get; set; }
        public bool? IsQuantitativeResultPassed { get; set; }
        public string? Remarks { get; set; }
    }
}
