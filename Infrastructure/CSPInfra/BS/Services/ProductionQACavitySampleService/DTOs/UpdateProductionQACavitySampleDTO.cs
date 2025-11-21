namespace BS.Services.ProductionQACavitySampleService.DTOs
{
    public class UpdateProductionQACavitySampleDTO
    {
        // UI: Sample Header
        public string Id { get; set; }
        public string? Name { get; set; }
        public DateTime InspectionDateTime { get; set; }
        public string? InspectionBy { get; set; }
        public string? ProductionQcId { get; set; }                 // OBSOLETE
        public bool? IsSamplePassed { get; set; }
        public bool IsActive { get; set; } = true;

        // UI: Inspection Rows
        public List<InspectionModifyObject> InspectionObjects { get; set; } = [];
    }

    public class InspectionModifyObject
    {
        public string? Id { get; set; }                             //  CURRENTLY UNUSED Production_QA_Sample_Result ID
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
