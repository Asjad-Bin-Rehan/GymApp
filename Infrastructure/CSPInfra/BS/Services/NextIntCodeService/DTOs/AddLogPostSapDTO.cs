namespace BS.Services.NextIntCodeService.DTOs
{
    public class AddLogPostSapDTO
    {
        public string? QType { get; set; }              // enum: purchase_qc, production_qc, production_qa
        public int? QCode { get; set; }                 // IntCode of Purchase/Production_QC/QA
        public bool? OverallStatus { get; set; }
        public bool? IsPostedToSap { get; set; }
        public bool? IsClosed { get; set; }
        public bool IsPerformed { get; set; }
        public string? BMR { get; set; }
        public string? Remarks { get; set; }
        public int? SamplesPassedCount { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public string? AnalyzedBy { get; set; }
        public int? InspectionQuantity { get; set; }
        public int? SampleQuantity { get; set; }
        public DateTime? ReportReviewDate { get; set; }
        public DateTime? ReportNextReviewDate { get; set; }
        public string? ReportRemarks { get; set; }

        // Log-FK
        public string? QId { get; set; }
    }
}
