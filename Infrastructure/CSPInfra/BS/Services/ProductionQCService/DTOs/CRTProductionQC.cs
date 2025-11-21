using DM.DomainModels;

namespace BS.Services.ProductionQCService.DTOs
{
    public static class CRTProductionQC
    {
        public static Production_QC ToDomain(this AddProductionQCDTO request, string userId)
        {
            return new Production_QC()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false,

                // FKs
                ItemId = request.ItemId,

                // Statuses
                OverallStatus = request.OverallStatus,
                IsPostedToSap = request.IsPostedToSap,
                IsPerformed = request.IsPerformed,
                IsClosed = request.IsClosed,

                // Inspection
                OperatedBy = request.OperatedBy,
                Barcode = request.Barcode,
                BatchNo = request.BatchNo,
                IsBarcodeGenerated = request.IsBarcodeGenerated,
                InspectionDateTime = request.InspectionDateTime,
                AnalyzedBy = request.AnalyzedBy,
                Remarks = request.Remarks,
                ReportReviewDate = request.ReportReviewDate,
                ReportNextReviewDate = request.ReportNextReviewDate,
                ReportRemarks = request.ReportRemarks,
                InspectionQuantity = request.InspectionQuantity,
                SampleQuantity = request.SampleQuantity,
                ReceiptQuantity = request.ReceiptQuantity,
                ProductionDate = request.ProductionDate,
                ProducedQuantity = request.ProducedQuantity,
                ProductionShift = request.ProductionShift,

                // SAP Doc
                DocNo = request.DocNo,
                DocType = request.DocType,
                DocDate = request.DocDate,
                DocEntry = request.DocEntry,
                BMRNo = request.BMRNo,
                BMRLoc = request.BMRLoc,
                StageType = request.StageType,

                // SAP Quantities
                OpenQuantity = request.OpenQuantity,
                ReceiveQuantity = request.ReceiveQuantity,
                PlannedQuantity = request.PlannedQuantity,
                CompletedQuantity = request.CompletedQuantity,
                RejectedQuantity = request.RejectedQuantity,

                // SAP Misc
                Status = request.Status,
                ItemCode = request.ItemCode,
                ItemDescription = request.ItemDescription,
                CardCode = request.CardCode,
                CardName = request.CardName,
                QcLotNo = request.QcLotNo,
                Warehouse = request.Warehouse,
                Vendor = request.Vendor,
                UoM = request.UoM,
                InventoryUoM = request.InventoryUoM,
                ProductionOrderStatus = request.ProductionOrderStatus,

                // Production Details
                Variant = request.Variant,
                ItemWeight = request.ItemWeight,
                Cavity = request.Cavity,
                CavityNo = request.CavityNo,
                CycleTime = request.CycleTime,
                Shift = request.Shift,
                MachineNo = request.MachineNo,
                MouldNo = request.MouldNo,
            };
        }

        public static ResponseProductionQCWithItem ToResponse(this Production_QC row)
        {
            return new ResponseProductionQCWithItem
            {
                Id = row.Id,
                IntCode = row.IntCode,
                IsActive = row.IsActive,
                CreatedBy = row.CreatedBy,
                CreatedDate = row.CreatedDate,
                UpdatedBy = row.UpdatedBy,
                UpdatedDate = row.UpdatedDate,
                IsArchived = row.IsArchived,

                // Statuses
                IsClosed = row.IsClosed,
                OverallStatus = row.OverallStatus,
                IsPerformed = row.IsPerformed,
                IsPostedToSap = row.IsPostedToSap,

                // Inspection
                OperatedBy = row.OperatedBy,
                Barcode = row.Barcode,
                BatchNo = row.BatchNo,
                IsBarcodeGenerated = row.IsBarcodeGenerated,
                InspectionDateTime = row.InspectionDateTime,
                AnalyzedBy = row.AnalyzedBy,
                Remarks = row.Remarks,
                ReportReviewDate = row.ReportReviewDate,
                ReportNextReviewDate = row.ReportNextReviewDate,
                ReportRemarks = row.ReportRemarks,
                InspectionQuantity = row.InspectionQuantity,
                SampleQuantity = row.SampleQuantity,
                ReceiptQuantity = row.ReceiptQuantity,
                ProductionDate = row.ProductionDate,
                ProducedQuantity = row.ProducedQuantity,
                ProductionShift = row.ProductionShift,

                // SAP Doc
                DocNo = row.DocNo,
                DocType = row.DocType,
                DocDate = row.DocDate,
                DocEntry = row.DocEntry,
                BMRNo = row.BMRNo,
                BMRLoc = row.BMRLoc,
                StageType = row.StageType,

                // SAP Quantities
                OpenQuantity = row.OpenQuantity,
                ReceiveQuantity = row.ReceiveQuantity,
                PlannedQuantity = row.PlannedQuantity,
                CompletedQuantity = row.CompletedQuantity,
                RejectedQuantity = row.RejectedQuantity,

                // SAP Misc
                Status = row.Status,
                QcLotNo = row.QcLotNo,
                Warehouse = row.Warehouse,
                Vendor = row.Vendor,
                UoM = row.UoM,
                ItemDescription = row.ItemDescription,
                ItemCode = row.ItemCode,
                CardCode = row.CardCode,
                CardName = row.CardName,

                // Production Details
                InventoryUoM = row.InventoryUoM,
                ProductionOrderStatus = row.ProductionOrderStatus,
                Variant = row.Variant,
                ItemWeight = row.ItemWeight,
                Cavity = row.Cavity,
                CavityNo = row.CavityNo,
                CycleTime = row.CycleTime,
                Shift = row.Shift,
                MachineNo = row.MachineNo,
                MouldNo = row.MouldNo,

                ItemDetails = row.Item == null ? new() : new()
                {
                    Id = row.Item.Id,
                    IntCode = row.Item.IntCode,
                    IsActive = row.Item.IsActive,
                    CreatedBy = row.Item.CreatedBy,
                    CreatedDate = row.Item.CreatedDate,
                    UpdatedBy = row.Item.UpdatedBy,
                    UpdatedDate = row.Item.UpdatedDate,
                    IsArchived = row.Item.IsArchived,

                    ItemCode = row.Item.ItemCode,
                    Name = row.Item.Name,
                    Type = row.Item.Type,
                    GroupCode = row.Item.GroupCode,
                    U_QACard = row.Item.U_QACard,
                    UoMGroupEntry = row.Item.UoMGroupEntry,
                    IsEnabledForQA = row.Item.IsEnabledForQA,
                    IsBatch = row.Item.IsBatch,
                    GroupName = row.Item.GroupName,
                    ManageBatchNumbers = row.Item.ManageBatchNumbers,
                    PackSize = row.Item.PackSize,
                }
            };
        }

        public static List<ResponseProductionQCWithItem> ToResponseList(this IEnumerable<Production_QC> rows)
        {
            return rows.Select(ToResponse).ToList();
        }
    }
}
