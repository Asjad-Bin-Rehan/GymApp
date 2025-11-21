using Helpers.CommonModels;

namespace BS.Services.ProductionQCService.DTOs
{
    public class ResponseProductionQCWithItem : ActivityTrackersInResponse
    {
        // Inspection
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public string? AnalyzedBy { get; set; }
        public string? OperatedBy { get; set; }
        public string? Barcode { get; set; }
        public string? BatchNo { get; set; }
        public bool? IsBarcodeGenerated { get; set; }
        public string? Remarks { get; set; }
        public DateTime? ReportReviewDate { get; set; }
        public DateTime? ReportNextReviewDate { get; set; }
        public string? ReportRemarks { get; set; }
        public double? InspectionQuantity { get; set; }
        public double? SampleQuantity { get; set; }
        public double? ReceiptQuantity { get; set; }
        public string? ProductionDate { get; set; }
        public double? ProducedQuantity { get; set; }
        public string? ProductionShift { get; set; }

        // Statuses
        public bool? IsPerformed { get; set; }
        public bool? IsPostedToSap { get; set; }
        public bool? IsClosed { get; set; }
        public bool? OverallStatus { get; set; }

        // SAP Doc
        public string? DocNo { get; set; }
        public string? DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public string? DocEntry { get; set; }
        public string? BMRNo { get; set; }
        public string? BMRLoc { get; set; }
        public string? StageType { get; set; }

        // SAP Quantities
        public double? OpenQuantity { get; set; }
        public double? ReceiveQuantity { get; set; }
        public double? PlannedQuantity { get; set; }
        public double? CompletedQuantity { get; set; }
        public double? RejectedQuantity { get; set; }

        // SAP Misc
        public string? Status { get; set; }
        public string? QcLotNo { get; set; }
        public string? Warehouse { get; set; }
        public string? Vendor { get; set; }
        public string? UoM { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }

        // Production Details
        public string? InventoryUoM { get; set; }
        public string? ProductionOrderStatus { get; set; }
        public string? Variant { get; set; }
        public string? Cavity { get; set; }
        public double? CavityNo { get; set; }
        public string? ItemWeight { get; set; }
        public double? CycleTime { get; set; }
        public string? Shift { get; set; }
        public string? MachineNo { get; set; }
        public string? MouldNo { get; set; }

        public ItemDetails ItemDetails { get; set; } = new();
    }

    public class ItemDetails : ActivityTrackersInResponse
    {
        public string? Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? ItemCode { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? GroupCode { get; set; }
        public string? U_QACard { get; set; }
        public string? UoMGroupEntry { get; set; }
        public bool? IsEnabledForQA { get; set; }
        public string? GroupName { get; set; }
        public string? ManageBatchNumbers { get; set; }
        public bool? IsBatch { get; set; }
        public double? PackSize { get; set; }
    }
}
