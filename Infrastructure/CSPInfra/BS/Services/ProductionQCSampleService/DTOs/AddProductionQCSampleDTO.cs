namespace BS.Services.ProductionQCSampleService.DTOs
{
    public class AddProductionQCSampleDTO
    {
        // UI: Sample Header
        public DateTime InspectionDateTime { get; set; }
        public string? InspectionBy { get; set; }
        public int? InspectionQuantity { get; set; }
        public string? QcId { get; set; }
        public string? Name { get; set; }
        public bool? IsSamplePassed { get; set; }

        // UI: Inspection Rows
        public List<InspectionObject> InspectionObjects { get; set; } = [];
    }

    public class InspectionObject
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
