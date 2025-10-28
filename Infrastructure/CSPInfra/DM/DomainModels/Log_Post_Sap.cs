using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Log_Post_Sap : Base<string>
    {
        // Columns

        public string? QType { get; set; }              // enum: purchase_qc, production_qc, production_qa
        public int? QCode { get; set; }                 // IntCode of Purchase/Production_QC/QA
        public string? BMR { get; set; }
        public bool? OverallStatus { get; set; }
        public bool? IsPerformed { get; set; }
        public bool? IsPostedToSap { get; set; }
        public bool? IsClosed { get; set; }
        public int? SamplesPassedCount { get; set; }

        public string? Remarks { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public string? AnalyzedBy { get; set; }
        public double? InspectionQuantity { get; set; }
        public double? SampleQuantity { get; set; }
        public DateTime? ReportReviewDate { get; set; }
        public DateTime? ReportNextReviewDate { get; set; }
        public string? ReportRemarks { get; set; }

        // Log-FK
        public string? QId { get; set; }

        // Navigation
        public Purchase_QC? Purchase_QC { get; set; }
        public Production_QC? Production_QC { get; set; }
        public Production_QA? Production_QA { get; set; }
    }
}
