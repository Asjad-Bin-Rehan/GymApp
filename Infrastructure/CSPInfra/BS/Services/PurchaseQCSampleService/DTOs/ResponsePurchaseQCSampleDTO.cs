using Helpers.CommonModels;

namespace BS.Services.PurchaseQCSampleService.DTOs
{
    public class ResponsePurchaseQCSampleDTO : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? QcId { get; set; }
        public bool? IsSamplePassed { get; set; }

        // UI: Sample Header
        public string? Name { get; set; }
        public DateTime InspectionDateTime { get; set; }
        public string? InspectionBy { get; set; }
        public int? InspectionQuantity { get; set; }

        // UI: Inspection Rows
        public List<InspectionResults> InspectionObjects { get; set; } = [];
    }

    public class InspectionResults : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? QcSampleId { get; set; }
        // only one of the two below will be NOT NULL in an object
        public string? QualitativeInspectionMappingId { get; set; }
        public string? QuantitativeInspectionMappingId { get; set; }
        // only two of the four below can be NOT NULL in an object
        public string? QualitativeResultId { get; set; }
        public bool? IsQualitativeResultPassed { get; set; }
        public double? QuantitativeResult { get; set; }
        public bool? IsQuantitativeResultPassed { get; set; }
        public string? Remarks { get; set; }
    }
}
