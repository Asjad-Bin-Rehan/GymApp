using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Production_QC_Sample : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? Name { get; set; }
        public DateTime InspectionDateTime { get; set; }
        public string? InspectionBy { get; set; }
        public int? InspectionQuantity { get; set; }
        public bool? IsSamplePassed { get; set; }

        // FK
        public string? QcId { get; set; }

        // Navigation
        public Production_QC? Production_QC { get; set; }
        public ICollection<Production_QC_Sample_Result> Production_QC_Sample_Results { get; set; } = [];
    }
}
