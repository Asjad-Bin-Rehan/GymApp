namespace BS.Services.PurchaseQCService.DTOs
{
    public class AddPurchaseQCDTO
    {
        // FKs
        public string? ItemId { get; set; }

        // Statuses
        public bool? IsPerformed { get; set; }
        public bool? IsPostedToSap { get; set; }
        public bool? IsClosed { get; set; }
        public bool? OverallStatus { get; set; }

        // Inspection
        public string? OperatedBy { get; set; }
        public string? Barcode { get; set; }
        public string? BatchNo { get; set; }
        public bool? IsBarcodeGenerated { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public string? AnalyzedBy { get; set; }
        public string? Remarks { get; set; }
        public DateTime? ReportReviewDate { get; set; }
        public DateTime? ReportNextReviewDate { get; set; }
        public string? ReportRemarks { get; set; }
        public double? InspectionQuantity { get; set; }
        public double? SampleQuantity { get; set; }
        public double? ReceiptQuantity { get; set; }
        public string? QcLotNo { get; set; }

        // SAP Doc
        public string? DocNo { get; set; }
        public string? DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public string? DocEntry { get; set; }
        public int? LineNo { get; set; }
        public string? BMRNo { get; set; }
        public string? BMRLoc { get; set; }
        public string? StageType { get; set; }

        // SAP Quantities
        public double? OpenQuantity { get; set; }
        public double? ReceiveQuantity { get; set; }
        public double? SapQuantity { get; set; }
        public double? CompletedQuantity { get; set; }
        public double? RejectedQuantity { get; set; }

        // SAP Misc
        public string? Status { get; set; }
        public string? Warehouse { get; set; }
        public string? LineStatus { get; set; }
        public string? VatGroup { get; set; }
        public string? UoM { get; set; }
        public double? Price { get; set; }
        public string? Vendor { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
    }
}
