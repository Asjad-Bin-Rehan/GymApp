namespace BS.Services.ProductionQCSampleService.DTOs
{
    public class UpdateProductionQCSampleDTO
    {
        // UI: Sample Header
        public string Id { get; set; }
        public string? Name { get; set; }
        public DateTime InspectionDateTime { get; set; }
        public string? InspectionBy { get; set; }
        public string? QcId { get; set; }                               // OBSOLETE
        public bool? IsSamplePassed { get; set; }
        public bool IsActive { get; set; } = true;

        // UI: Inspection Rows
        public List<InspectionUpdateObject> InspectionObjects { get; set; } = [];
    }

    public class InspectionUpdateObject
    {
        public string? Id { get; set; }                                 // CURRENTLY UNUSED
        // only one of the two below can be NOT NULL in an object
        public string? QualitativeInspectionMappingId { get; set; }
        public string? QuantitativeInspectionMappingId { get; set; }
        // only two of the four below can be NOT NULL in an object
        public string? QualitativeResultId { get; set; }
        public bool? IsQualitativeResultPassed { get; set; }
        public double? QuantitativeResult { get; set; }
        public bool? IsQuantitativeResultPassed { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; } = true;                  // CURRENTLY UNUSED
    }
}
