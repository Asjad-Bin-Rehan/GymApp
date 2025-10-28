using DM.DomainModels;
using MediatR;

namespace BS.Services.PurchaseQCService.DTOs
{
    public static class CRTPurchase
    {
        public static Purchase_QC ToDomain(this AddPurchaseQCDTO request, string userId)
        {
            return new Purchase_QC()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false,

                // Statuses
                IsPerformed = request.IsPerformed,
                IsClosed = request.IsClosed,
                IsPostedToSap = request.IsPostedToSap,
                OverallStatus = request.OverallStatus,

                // Inspection
                Barcode = request.Barcode,
                BatchNo = request.BatchNo,
                IsBarcodeGenerated = request.IsBarcodeGenerated,
                OperatedBy = request.OperatedBy,
                InspectionDateTime = request.InspectionDateTime,
                AnalyzedBy = request.AnalyzedBy,
                Remarks = request.Remarks,
                InspectionQuantity = request.InspectionQuantity,
                SampleQuantity = request.SampleQuantity,
                ReportReviewDate = request.ReportReviewDate,
                ReportNextReviewDate = request.ReportNextReviewDate,
                ReportRemarks = request.ReportRemarks,

                // SAP Doc
                DocNo = request.DocNo,
                DocType = request.DocType,
                DocDate = request.DocDate,
                DocEntry = request.DocEntry,
                LineNo = request.LineNo,
                BMRNo = request.BMRNo,
                BMRLoc = request.BMRLoc,
                StageType = request.StageType,

                // SAP Quantities
                OpenQuantity = request.OpenQuantity,
                ReceiveQuantity = request.ReceiveQuantity,
                SapQuantity = request.SapQuantity,
                CompletedQuantity = request.CompletedQuantity,
                RejectedQuantity = request.RejectedQuantity,
                ReceiptQuantity = request.ReceiptQuantity,

                // SAP Misc
                Status = request.Status,
                QcLotNo = request.QcLotNo,
                Warehouse = request.Warehouse,
                LineStatus = request.LineStatus,
                VatGroup = request.VatGroup,
                UoM = request.UoM,
                Price = request.Price,
                Vendor = request.Vendor,
                CardCode = request.CardCode,
                CardName = request.CardName,
                ItemCode = request.ItemCode,
                ItemDescription = request.ItemDescription,

                // FK
                ItemId = request.ItemId
            };
        }

        public static ResponsePurchaseQCWithItem ToResponse(this Purchase_QC row)
        {
            return new ResponsePurchaseQCWithItem
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
                IsPerformed = row.IsPerformed,
                IsClosed = row.IsClosed,
                IsPostedToSap = row.IsPostedToSap,
                OverallStatus = row.OverallStatus,

                // Inspection
                Barcode = row.Barcode,
                BatchNo = row.BatchNo,
                IsBarcodeGenerated = row.IsBarcodeGenerated,
                OperatedBy = row.OperatedBy,
                InspectionDateTime = row.InspectionDateTime,
                AnalyzedBy = row.AnalyzedBy,
                Remarks = row.Remarks,
                ReportRemarks = row.ReportRemarks,
                ReportReviewDate = row.ReportReviewDate,
                ReportNextReviewDate = row.ReportNextReviewDate,
                InspectionQuantity = row.InspectionQuantity,
                SampleQuantity = row.SampleQuantity,
                ReceiptQuantity = row.ReceiptQuantity,

                // SAP Doc
                DocNo = row.DocNo,
                DocType = row.DocType,
                DocDate = row.DocDate,
                DocEntry = row.DocEntry,
                LineNo = row.LineNo,
                BMRNo = row.BMRNo,
                BMRLoc = row.BMRLoc,
                StageType = row.StageType,

                // SAP Quantities
                OpenQuantity = row.OpenQuantity,
                ReceiveQuantity = row.ReceiveQuantity,
                SapQuantity = row.SapQuantity,
                CompletedQuantity = row.CompletedQuantity,
                RejectedQuantity = row.RejectedQuantity,

                // SAP Misc
                Status = row.Status,
                QcLotNo = row.QcLotNo,
                Warehouse = row.Warehouse,
                LineStatus = row.LineStatus,
                VatGroup = row.VatGroup,
                UoM = row.UoM,
                Price = row.Price,
                Vendor = row.Vendor,
                CardCode = row.CardCode,
                CardName = row.CardName,
                ItemCode = row.ItemCode,
                ItemDescription = row.ItemDescription,

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

        public static List<ResponsePurchaseQCWithItem> ToResponseList(this IEnumerable<Purchase_QC> rows)
        {
            return rows.Select(ToResponse).ToList();
        }
    }
}
