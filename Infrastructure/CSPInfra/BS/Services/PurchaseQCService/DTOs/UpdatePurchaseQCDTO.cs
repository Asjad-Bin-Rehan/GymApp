using System.ComponentModel.DataAnnotations;

namespace BS.Services.PurchaseQCService.DTOs
{
    public class UpdatePurchaseQCDTO
    {
        public bool? IsPerformed { get; set; }
        public bool? IsPostedToSap { get; set; }
        public bool? IsClosed { get; set; }
        public bool? OverallStatus { get; set; }
        [Required] public string Id { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public string? Remarks { get; set; }
        public string? OperatedBy { get; set; }
        public string? Barcode { get; set; }
        public string? BatchNo { get; set; }
        public bool? IsBarcodeGenerated { get; set; }
        public DateTime? ReportReviewDate { get; set; }
        public DateTime? ReportNextReviewDate { get; set; }
        public string? ReportRemarks { get; set; }
        public double? ReceiptQuantity { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
