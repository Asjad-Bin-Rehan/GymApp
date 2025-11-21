using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Production_QA_Cavity_Sample : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? Name { get; set; }
        public DateTime InspectionDateTime { get; set; }
        public string? InspectionBy { get; set; }
        public int? InspectionQuantity { get; set; }
        public bool? IsSamplePassed { get; set; }

        // FK
        public string? CavityId { get; set; }

        // Navigation
        public Production_QA_Cavity? Production_QA_Cavity { get; set; }
        public ICollection<Production_QA_Cavity_Sample_Result> Production_QA_Cavity_Sample_Results { get; set; } = [];
    }
}
