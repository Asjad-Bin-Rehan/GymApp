using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Production_QC : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public bool? OverallStatus { get; set; }
        public bool? IsPerformed { get; set; }
        public bool? IsPostedToSap { get; set; }
        public bool? IsClosed { get; set; }

        public string? Barcode { get; set; }
        public string? BatchNo { get; set; }
        public bool? IsBarcodeGenerated { get; set; }

        public string? OperatedBy { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public string? AnalyzedBy { get; set; }
        public string? Remarks { get; set; }
        public DateTime? ReportReviewDate { get; set; }
        public DateTime? ReportNextReviewDate { get; set; }
        public string? ReportRemarks { get; set; }

        public string? QcLotNo { get; set; }
        public double? SampleQuantity { get; set; }
        public double? InspectionQuantity { get; set; }
        public double? ReceiptQuantity { get; set; }
        public double? ProducedQuantity { get; set; }
        public string? ProductionDate { get; set; }
        public string? ProductionShift { get; set; }

        public string? DocNo { get; set; }
        public DateTime? DocDate { get; set; }
        public string? DocType { get; set; }    // enum purchase order, or production order
        public string? DocEntry { get; set; }
        public string? StageType { get; set; }

        public double? OpenQuantity { get; set; }
        public double? ReceiveQuantity { get; set; }
        public double? PlannedQuantity { get; set; }
        public double? CompletedQuantity { get; set; }
        public double? RejectedQuantity { get; set; }

        public string? BMRNo { get; set; }
        public string? BMRLoc { get; set; }

        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }

        public string? Status { get; set; }
        public string? Warehouse { get; set; }
        public string? Vendor { get; set; }
        public string? Variant { get; set; }
        public string? Shift { get; set; }
        public string? ItemWeight { get; set; }
        public string? Cavity { get; set; }
        public double? CavityNo { get; set; }
        public double? CycleTime { get; set; }
        public string? MachineNo { get; set; }
        public string? MouldNo { get; set; }

        public string? UoM { get; set; }
        public string? InventoryUoM { get; set; }
        public string? ProductionOrderStatus { get; set; }

        // FKs
        public string? ItemId { get; set; }

        // Navigation
        public Item? Item { get; set; }
        public ICollection<Production_QC_Sample> Production_QC_Samples { get; set; } = [];
        public ICollection<Log_Post_Sap> Log_Post_Saps { get; set; } = [];
    }
}
