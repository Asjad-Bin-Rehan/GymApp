using BS.Services.PurchaseQCService.DTOs;
using DA;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.PurchaseQCService
{
    public class PurchaseQCService : IPurchaseQCService
    {
        private readonly IUnitOfWork _uow;

        public PurchaseQCService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<bool> AddPurchaseQC(AddPurchaseQCDTO request, string userId, CancellationToken ct)
        {
            var entity = request.ToDomain(userId);
            await _uow.purchase_qc.AddAsync(entity, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponsePurchaseQCWithItem>> ListAllPurchaseQCsWithItem(CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.purchase_qc.GetQueryable();

            var result = await query.Data
                .Include(x => x.Item)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(skipRecords)
                .Take(lastCount)
                .ToListAsync(ct);

            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");

            var data = result.ToResponseList().ToList();
            return data;
        }

        public async Task<ListPurchaseQCResponse> ListAllPurchaseQCs(int pageSize, int pageNumber, CancellationToken ct)
        {
            int skipRecords = ((pageNumber==0 ? 1 : pageNumber) - 1) * pageSize;

            var query = _uow.purchase_qc.GetQueryable();
            var result = await query.Data
                .Include(x => x.Item)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(skipRecords)
                .Take(pageSize)
                .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No data retrieved from the response.");
            var data = result.ToResponseList().ToList();

            return new ListPurchaseQCResponse
            {
                PageSize = pageSize,
                PageNumber = pageNumber == 0 ? 1 : pageNumber,
                Values = data
            };
        }

        public async Task<int> GetTotalRecords(CancellationToken ct)
        {
            return await _uow.purchase_qc.GetQueryable().Data.CountAsync(ct);
        }

        public async Task<ResponsePurchaseQCWithItem> GetPurchaseQCWithItemById(string Id, CancellationToken ct)
        {
            var query = _uow.purchase_qc.GetQueryable();
            var result = await query.Data.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id==Id);
            ArgumentFalseException.ThrowIfFalse(result!=null, "No records found");
            var data = result?.ToResponse();
            return data;
        }

        public async Task<double> GetSampleQuantity(string itemId, int inspectionQuantity, CancellationToken ct)
        {
            var query = _uow.item_sample.GetQueryable();
            var result = await query.Data.Include(x => x.Sampling_Ranges).FirstOrDefaultAsync(x => x.ItemId == itemId && x.IsActive);
            ArgumentFalseException.ThrowIfFalse(result != null, "No records found");
            foreach (var samplingRange in result?.Sampling_Ranges ?? [])
            {
                if (samplingRange.LotSizeMin <= inspectionQuantity && inspectionQuantity <= samplingRange.LotSizeMax && samplingRange.IsActive)
                {
                    return samplingRange.SampleQty ?? 0;
                }
            }
            return 0;
        }

        public async Task<ResponsePurchaseQCWithItem> GetPurchaseQCId(string itemCode, string? docNumber, int? lineNo, string? stageType, CancellationToken ct)
        {
            var query = _uow.purchase_qc.GetQueryable();
            var result = await query.Data
                .Where(qc => qc.Item!=null && qc.Item.ItemCode==itemCode && (docNumber==null || qc.DocNo==docNumber) && (lineNo==null || qc.LineNo==lineNo) && (stageType==null || qc.StageType==stageType))
                .OrderByDescending(qc => qc.CreatedDate)
                .FirstOrDefaultAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result!=null, "No records found");
            return result.ToResponse(); 
        }

        public async Task<bool> UpdatePurchaseQC(UpdatePurchaseQCDTO request, string userId, CancellationToken ct)
        {
            var query = _uow.purchase_qc.GetQueryable();
            var result = await query.Data.Where(x => x.Id == request.Id).SingleOrDefaultAsync(ct) ?? throw new ArgumentFalseException("No active record found to update");
            result.InspectionDateTime = request.InspectionDateTime;
            result.OverallStatus = request.OverallStatus;
            result.Remarks = request.Remarks;
            result.IsPerformed = request.IsPerformed;
            result.IsPostedToSap = request.IsPostedToSap;
            result.IsClosed = request.IsClosed;
            result.OperatedBy = request.OperatedBy;
            result.Barcode = request.Barcode;
            result.BatchNo = request.BatchNo;
            result.IsBarcodeGenerated = request.IsBarcodeGenerated;
            result.ReportReviewDate = request.ReportReviewDate;
            result.ReportNextReviewDate = request.ReportNextReviewDate;
            result.ReportRemarks = request.ReportRemarks;
            result.ReceiptQuantity = request.ReceiptQuantity;
            result.IsActive = request.IsActive;
            await _uow.purchase_qc.UpdateAsync(result, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        #region Custom FluentValidations
        public async Task<bool> IsItemIdAvailable(string? Id, CancellationToken ct)
        {
            var result = await _uow.item.AnyAsync(ct, x => x.Id == Id);
            return result.Data;
        }
        #endregion Custom FluentValidations
    }
}
