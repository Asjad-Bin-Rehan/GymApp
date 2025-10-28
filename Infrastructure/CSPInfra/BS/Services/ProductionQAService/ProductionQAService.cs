using System.Text.RegularExpressions;
using BS.EnumsAndConstants.Constant;
using BS.Services.ProductionQAService.DTOs;
using BS.Services.ProductionQCService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.ProductionQAService
{
    public class ProductionQAService : IProductionQAService
    {
        private readonly IUnitOfWork _uow;

        public ProductionQAService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<bool> AddProductionQA(AddProductionQADTO request, string userId, CancellationToken ct)
        {
            var entity = request.ToDomain(userId);
            await _uow.production_qa.AddAsync(entity, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponseProductionQAWithItem>> ListAllProductionQAsWithItem(CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.production_qa.GetQueryable();
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

        public async Task<ListProductionQAResponse> ListAllProductionQAs(int pageSize, int pageNumber, CancellationToken ct)
        {
            int skipRecords = ((pageNumber == 0 ? 1 : pageNumber) - 1) * pageSize;

            var query = _uow.production_qa.GetQueryable();
            var result = await query.Data
                .Include(x => x.Item)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(skipRecords)
                .Take(pageSize)
                .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No data retrieved from the response.");
            var data = result.ToResponseList().ToList();

            return new ListProductionQAResponse
            {
                PageSize = pageSize,
                PageNumber = pageNumber == 0 ? 1 : pageNumber,
                Values = data
            };
        }

        public async Task<int> GetTotalRecords(CancellationToken ct)
        {
            return await _uow.production_qa.GetQueryable().Data.CountAsync(ct);
        }
        
        public async Task<ResponseProductionQAWithItem> GetProductionQAWithItemById(string id, CancellationToken ct)
        {
            var query = _uow.production_qa.GetQueryable();
            var result = await query.Data.Include(x => x.Item).SingleOrDefaultAsync(x => x.Id==id);
            ArgumentFalseException.ThrowIfFalse(result != null, "No records found");
            return result.ToResponse();
        }

        public async Task<ResponseProductionQAWithItem> GetProductionQAId(string itemCode, string? docNumber, string? stageType, CancellationToken ct)
        {
            var query = _uow.production_qa.GetQueryable();
            var result = await query.Data
                .Where(qc => qc.Item != null && qc.Item.ItemCode == itemCode && (docNumber==null || qc.DocNo==docNumber) && (stageType==null || qc.StageType==stageType))
                .OrderByDescending(qc => qc.CreatedDate)
                .FirstOrDefaultAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result != null, "No records found");
            return result.ToResponse();
        }

        public async Task<bool> UpdateProductionQA(UpdateProductionQADTO request, string userId, CancellationToken ct)
        {
            var query = _uow.production_qa.GetQueryable();
            var result = await query.Data.Where(x => x.Id == request.Id).SingleOrDefaultAsync(ct)  ?? throw new ArgumentFalseException("No active record found to update");
            result.InspectionDateTime = request.InspectionDateTime;
            result.Remarks = request.Remarks;
            result.Barcode = request.Barcode;
            result.BatchNo = request.BatchNo;
            result.IsBarcodeGenerated = request.IsBarcodeGenerated;
            result.OperatedBy = request.OperatedBy;
            result.OverallStatus = request.OverallStatus;
            result.IsPostedToSap = request.IsPostedToSap;
            result.IsPerformed = request.IsPerformed;
            result.IsClosed = request.IsClosed;
            result.ReportRemarks = request.ReportRemarks;
            result.ReportReviewDate = request.ReportReviewDate;
            result.ReportNextReviewDate = request.ReportNextReviewDate;
            result.ReceiptQuantity = request.ReceiptQuantity;
            result.ProductionDate = request.ProductionDate;
            result.ProductionShift = request.ProductionShift;
            result.IsActive = request.IsActive;
            await _uow.production_qa.UpdateAsync(result, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<string?> GetProductionQaBMRByQaId(string qaId, CancellationToken ct)
        {
            var query = _uow.production_qa.GetQueryable();
            var result = await query.Data.Include(x => x.Item).SingleOrDefaultAsync(x => x.Id == qaId) ?? throw new ArgumentFalseException("No records found");

            string? itemCodeFirstChar = result?.Item?.ItemCode is { Length: >= 1 } x ? x[..1] : result?.Item?.ItemCode;
            string? itemCodeLast4Digits = Regex.Match(result?.Item?.ItemCode ?? string.Empty, @"\d{4}(?!.*\d)").Value;
            string? bmrLocFirstChar = result?.BMRLoc is { Length: >= 1 } b ? b[..1] : result?.BMRLoc;
            string? yearSuffix = (result?.CreatedDate.Year % 100)?.ToString("D2");
            string? bmrNoLast3Digits = Regex.Match(result?.BMRNo ?? string.Empty, @"\d{3}(?!.*\d)").Value;
            string? machineNoLast3Digits = Regex.Match(result?.MachineNo ?? string.Empty, @"\d{3}(?!.*\d)").Value;

            return $"{itemCodeFirstChar}{itemCodeLast4Digits}{bmrLocFirstChar}{yearSuffix}{bmrNoLast3Digits}{machineNoLast3Digits}";
        }

        public async Task<List<ResponseProductionQaReportCavityWise>> GetProductionQaReportCavityWiseById(string qaId, CancellationToken ct)
        {
            var productionQA = _uow.production_qa.GetQueryable();
            var qaCavities = _uow.production_qa_cavity.GetQueryable();
            var cavitySamples = _uow.production_qa_cavity_sample.GetQueryable();
            var sampleResults = _uow.production_qa_cavity_sample_result.GetQueryable();
            var quantitativeMapping = _uow.quantitative_inspection_mapping.GetQueryable();
            var qualitativeMapping = _uow.qualitative_inspection_mapping.GetQueryable();
            var characteristics = _uow.inspection_characteristic.GetQueryable();
            var items = _uow.item.GetQueryable();

            var comprehensionQuery = from cavity in qaCavities.Data
                        where cavity.QaId == qaId
                        join qa in productionQA.Data on cavity.QaId equals qa.Id
                        join item in items.Data on qa.ItemId equals item.Id
                        join sample in cavitySamples.Data on cavity.Id equals sample.CavityId into sampleGroup
                        from sample in sampleGroup.DefaultIfEmpty()
                        join result in sampleResults.Data on sample.Id equals result.QaCavitySampleId into resultGroup
                        from result in resultGroup.DefaultIfEmpty()
                        select new { cavity, qa, item, sample, result };

            var listedComprehensionQuery = await comprehensionQuery.ToListAsync(ct);

            var query = listedComprehensionQuery
                .GroupBy(x => x.cavity.Name)
                .Select(g => new ResponseProductionQaReportCavityWise
                {
                    CavityName = g.Key,
                    DocNo = g.Min(x => x.qa.DocNo),
                    InspectionDateTime = g.Min(x => x.qa.InspectionDateTime),
                    ItemCode = g.Min(x => x.item.ItemCode),
                    AreCavitiesPassed = g.All(x => x.cavity.IsCavityPassed == true),
                    AreSamplesPassed = g.Where(x => x.sample != null).All(x => x.sample.IsSamplePassed == true),
                    SampleObjects = g
                        .Where(x => x.sample != null)
                        .OrderBy(x => x.sample.InspectionDateTime)
                        .Select(x => new SampleObject
                        {
                            IntCode = x.sample.IntCode,
                            Name = x.sample.Name,
                            InspectionDateTime = x.sample.InspectionDateTime,
                            Type = x.result?.QualitativeInspectionMappingId == null ? KConstantInspectionTypeBS.quantitative : KConstantInspectionTypeBS.qualitative,
                            Result = x.result?.QualitativeInspectionMappingId == null
                                ? x.result?.IsQuantitativeResultPassed
                                : x.result?.IsQualitativeResultPassed,
                            Characteristic = x.result?.QualitativeInspectionMappingId == null
                                ? (from map in quantitativeMapping.Data
                                   join ch in characteristics.Data on map.CharacteristicId equals ch.Id
                                   where map.Id == x.result.QuantitativeInspectionMappingId
                                   select ch.Description).FirstOrDefault()
                                : (from map in qualitativeMapping.Data
                                   join ch in characteristics.Data on map.CharacteristicId equals ch.Id
                                   where map.Id == x.result.QualitativeInspectionMappingId
                                   select ch.Description).FirstOrDefault(),
                            QuantitativeResult = x.result?.QuantitativeResult
                        })
                        .ToList()
                })
                .OrderBy(x => x.CavityName)
                .ToList();

            return query;
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
