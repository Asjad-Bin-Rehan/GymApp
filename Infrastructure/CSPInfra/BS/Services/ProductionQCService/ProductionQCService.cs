using System.Text.RegularExpressions;
using BS.Services.ProductionQCService.DTOs;
using BS.Services.PurchaseQCService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.ProductionQCService
{
    public class ProductionQCService : IProductionQCService
    {
        private IUnitOfWork _uow;
        public ProductionQCService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<bool> AddProductionQC(AddProductionQCDTO request, string userId, CancellationToken ct)
        {
            var entity = request.ToDomain(userId);
            await _uow.production_qc.AddAsync(entity, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponseProductionQCWithItem>> ListAllProductionQCsWithItem(CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.production_qc.GetQueryable();

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

        public async Task<ListProductionQCResponse> ListAllProductionQCs(int pageSize, int pageNumber, CancellationToken ct)
        {
            int skipRecords = ((pageNumber == 0 ? 1 : pageNumber) - 1) * pageSize;

            var query = _uow.production_qc.GetQueryable();
            var result = await query.Data
                .Include(x => x.Item)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(skipRecords)
                .Take(pageSize)
                .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No data retrieved from the response.");
            var data = result.ToResponseList().ToList();

            return new ListProductionQCResponse
            {
                PageSize = pageSize,
                PageNumber = pageNumber==0 ? 1 : pageNumber,
                Values = data
            };
        }

        public async Task<int> GetTotalRecords(CancellationToken ct)
        {
            return await _uow.production_qc.GetQueryable().Data.CountAsync(ct);
        }

        public async Task<ResponseProductionQCWithItem> GetProductionQCWithItemById(string Id, CancellationToken ct)
        {
            var query = _uow.production_qc.GetQueryable();
            var result = await query.Data.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == Id);
            ArgumentFalseException.ThrowIfFalse(result != null, "No records found");
            var data = result?.ToResponse();
            return data;
        }

        public async Task<ResponseProductionQCWithItem> GetProductionQCId(string itemCode, string? docNumber, string? stageType, CancellationToken ct)
        {
            var query = _uow.production_qc.GetQueryable();
            var result = await query.Data
                .Where(qc => qc.Item!=null && qc.Item.ItemCode==itemCode && (docNumber==null || qc.DocNo==docNumber) && (stageType==null || qc.StageType==stageType))
                .OrderByDescending(qc => qc.CreatedDate)
                .FirstOrDefaultAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result!=null, "No records found");
            return result.ToResponse();
        }

        public async Task<bool> UpdateProductionQC(UpdateProductionQCDTO request, string userId, CancellationToken ct)
        {
            var query = _uow.production_qc.GetQueryable();
            var result = await query.Data.Where(x => x.Id == request.Id).SingleOrDefaultAsync(ct) ?? throw new ArgumentFalseException("No active record found to update");
            result.InspectionDateTime = request.InspectionDateTime;
            result.IsClosed = request.IsClosed;
            result.IsPerformed = request.IsPerformed;
            result.IsPostedToSap = request.IsPostedToSap;
            result.OverallStatus = request.OverallStatus;
            result.Remarks = request.Remarks;
            result.OperatedBy = request.OperatedBy;
            result.Barcode = request.Barcode;
            result.BatchNo = request.BatchNo;
            result.IsBarcodeGenerated = request.IsBarcodeGenerated;
            result.ReportRemarks = request.ReportRemarks;
            result.ReportReviewDate = request.ReportReviewDate;
            result.ReportNextReviewDate = request.ReportNextReviewDate;
            result.ReceiptQuantity = request.ReceiptQuantity;
            result.ProductionDate = request.ProductionDate;
            result.ProductionShift = request.ProductionShift;
            result.IsActive = request.IsActive;
            await _uow.production_qc.UpdateAsync(result, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<string?> GetProductionQcBMRByQcId(string qcId, CancellationToken ct)
        {
            var query = _uow.production_qc.GetQueryable();
            var result = await query.Data.Include(x => x.Item).SingleOrDefaultAsync(x => x.Id == qcId) ?? throw new ArgumentFalseException("No records found");

            string? itemCodeFirstChar = result?.Item?.ItemCode is { Length: >= 1 } x ? x[..1] : result?.Item?.ItemCode;
            string? itemCodeLast4Digits = Regex.Match(result?.Item?.ItemCode ?? string.Empty, @"\d{4}(?!.*\d)").Value;
            string? bmrLocFirstChar = result?.BMRLoc is { Length: >= 1 } b ? b[..1] : result?.BMRLoc;
            string? yearSuffix = (result?.CreatedDate.Year % 100)?.ToString("D2");
            string? bmrNoLast3Digits = Regex.Match(result?.BMRNo ?? string.Empty, @"\d{3}(?!.*\d)").Value;
            string? machineNoLast3Digits = Regex.Match(result?.MachineNo ?? string.Empty, @"\d{3}(?!.*\d)").Value;

            return $"{itemCodeFirstChar}{itemCodeLast4Digits}{bmrLocFirstChar}{yearSuffix}{bmrNoLast3Digits}{machineNoLast3Digits}";
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
