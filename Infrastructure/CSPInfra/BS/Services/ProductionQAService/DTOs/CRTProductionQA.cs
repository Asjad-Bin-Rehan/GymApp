using DM.DomainModels;

namespace BS.Services.ProductionQAService.DTOs
{
    public static class CRTProductionQA
    {
        public static Production_QA ToDomain(this AddProductionQADTO request, string userId)
        {
            return new Production_QA
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false,

                // Statuses
                OverallStatus = request.OverallStatus,
                IsPostedToSap = request.IsPostedToSap,
                IsPerformed = request.IsPerformed,
                IsClosed = request.IsClosed,

                // FKs
                ItemId = request.ItemId,

                // Inspection
                OperatedBy = request.OperatedBy,
                Barcode = request.Barcode,
                BatchNo = request.BatchNo,
                IsBarcodeGenerated = request.IsBarcodeGenerated,
                AnalyzedBy = request.AnalyzedBy,
                InspectionDateTime = request.InspectionDateTime,
                SampleQuantity = request.SampleQuantity,
                InspectionQuantity = request.InspectionQuantity,
                ReceiptQuantity = request.ReceiptQuantity,
                ProducedQuantity = request.ProducedQuantity,
                ProductionDate = request.ProductionDate,
                ProductionShift = request.ProductionShift,
                Remarks = request.Remarks,
                ReportReviewDate = request.ReportReviewDate,
                ReportNextReviewDate = request.ReportNextReviewDate,
                ReportRemarks = request.ReportRemarks,

                // SAP Doc
                DocNo = request.DocNum,
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
                QcLotNo = request.QcLotNo,
                Warehouse = request.Warehouse,
                Vendor = request.Vendor,
                UoM = request.UoM,
                ItemCode = request.ItemCode,
                ItemDescription = request.ItemDescription,
                CardCode = request.CardCode,
                CardName = request.CardName,

                // Production Details
                InventoryUoM = request.InventoryUoM,
                ProductionOrderStatus = request.ProductionOrderStatus,
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

        public static ResponseProductionQAWithItem ToResponse(this Production_QA row)
        {
            return new ResponseProductionQAWithItem
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
                IsPerformed = row.IsPerformed,
                IsPostedToSap = row.IsPostedToSap,
                OverallStatus = row.OverallStatus,

                // Inspection
                OperatedBy = row.OperatedBy,
                Barcode = row.Barcode,
                BatchNo = row.BatchNo,
                IsBarcodeGenerated = row.IsBarcodeGenerated,
                AnalyzedBy = row.AnalyzedBy,
                InspectionDateTime = row.InspectionDateTime,
                SampleQuantity = row.SampleQuantity,
                InspectionQuantity = row.InspectionQuantity,
                ReceiptQuantity = row.ReceiptQuantity,
                ProducedQuantity = row.ProducedQuantity,
                ProductionDate = row.ProductionDate,
                ProductionShift = row.ProductionShift,
                Remarks = row.Remarks,
                ReportReviewDate = row.ReportReviewDate,
                ReportNextReviewDate = row.ReportNextReviewDate,
                ReportRemarks = row.ReportRemarks,

                // SAP Doc
                DocNum = row.DocNo,
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

                    CreatedBy = row.Item.CreatedBy,
                    CreatedDate = row.Item.CreatedDate,
                    UpdatedBy = row.Item.UpdatedBy,
                    UpdatedDate = row.Item.UpdatedDate,
                    IsActive = row.Item.IsActive,
                    IsArchived = row.Item.IsArchived
                }
            };
        }

        public static List<ResponseProductionQAWithItem> ToResponseList(this IEnumerable<Production_QA> rows)
        {
            return rows.Select(ToResponse).ToList();
        }
    }
}
