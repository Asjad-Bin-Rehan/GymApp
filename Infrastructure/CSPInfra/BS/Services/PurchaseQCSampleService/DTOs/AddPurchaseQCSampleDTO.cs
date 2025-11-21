namespace BS.Services.PurchaseQCSampleService.DTOs
{
    public class AddPurchaseQCSampleDTO
    {
        // UI: Sample Header
        public string? Name { get; set; }
        public DateTime InspectionDateTime { get; set; }
        public string? InspectionBy { get; set; }
        public string? QcId { get; set; }
        public bool? IsSamplePassed { get; set; }

        // UI: Inspection Rows
        public List<InspectionObjects> InspectionObjects { get; set; } = [];
    }

    public class InspectionObjects
    {
        // only one of the two below can be NOT NULL in an object
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