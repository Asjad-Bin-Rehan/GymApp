using BS.Services.ProductionQCService.DTOs;
using BS.Services.PurchaseQCSampleService.DTOs;
using DA;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.PurchaseQCSampleService
{
    public class PurchaseQCSampleService : IPurchaseQCSampleService
    {
        private IUnitOfWork _uow;
        public PurchaseQCSampleService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<bool> AddPurchaseQCSample(AddPurchaseQCSampleDTO request, string userId, CancellationToken ct)
        {
            var qcSample = request.ToDomain(userId);
            await _uow.purchase_qc_sample.AddAsync(qcSample, userId, ct);

            if (request.InspectionObjects != null && request.InspectionObjects.Any())
            {
                foreach (var inspectionObject in request.InspectionObjects)
                {
                    var sampleResult = inspectionObject.ToDomain(userId, qcSample.Id);
                    await _uow.purchase_qc_sample_result.AddAsync(sampleResult, userId, ct);

                    var logResult = sampleResult.ToDomain(userId, qcSample.IsSamplePassed);
                    await _uow.log_purchase_qc_sample_result.AddAsync(logResult, userId, ct);
                }
            }
            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponsePurchaseQCSampleDTO>> ListAllPurchaseQCSamplesByQcId(string qcId, CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.purchase_qc_sample.GetQueryable();

            var result = await query.Data
                .Where(x => x.QcId == qcId)
                .Include(x => x.Purchase_QC_Sample_Results)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(skipRecords)
                .Take(lastCount)
                .ToListAsync(ct);

            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            return result.ToResponseList();
        }

        public async Task<List<ResponsePurchaseQCSampleDTO>> GetPurchaseQCSampleById(CancellationToken ct, string pruchaseQCSampleId)
        {
            var query = _uow.purchase_qc_sample.GetQueryable();
            var result = await query.Data
                                    .Include(x => x.Purchase_QC_Sample_Results)
                                    .Where(x => x.Id == pruchaseQCSampleId)
                                    .OrderByDescending(x => x.CreatedDate)
                                    .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            var data = result.ToResponseList().ToList();
            return data;
        }

        public async Task<bool> UpdatePurchaseQCSample(UpdatePurchaseQCSampleDTO request, string userId, CancellationToken ct)
        {
            // Fetch the Sample
            var query = _uow.purchase_qc_sample.GetQueryable();
            var result = await query.Data.Include(x => x.Purchase_QC_Sample_Results).Where(x => x.Id == request.Id).SingleOrDefaultAsync(ct) ?? throw new ArgumentFalseException("No active record found to update");
            var existingSamplePassStatus = result.IsSamplePassed;

            // Update the Header Fields
            result.Name = request.Name;
            result.InspectionDateTime = request.InspectionDateTime;
            result.InspectionBy = request.InspectionBy;
            result.IsSamplePassed = request.IsSamplePassed;
            result.IsActive = request.IsActive;
            await _uow.purchase_qc_sample.UpdateAsync(result, userId, ct);

            foreach (var inspectionObject in request.InspectionObjects)
            {
                // Fetch the Sample Results
                var resultObject = result.Purchase_QC_Sample_Results.FirstOrDefault(x => // FE cant x.Id == inspectionObject.Id
                    x.QcSampleId == request.Id
                    && x.QualitativeInspectionMappingId == inspectionObject.QualitativeInspectionMappingId
                    && x.QuantitativeInspectionMappingId == inspectionObject.QuantitativeInspectionMappingId
                );
                
                // Log the Sample Results & Pass-Status Header Field
                var logResultObject = resultObject?.ToDomain(userId, existingSamplePassStatus);
                await _uow.log_purchase_qc_sample_result.AddAsync(logResultObject, userId, ct);

                // Update the Sample Results
                resultObject.QualitativeResultId = inspectionObject.QualitativeResultId;
                resultObject.IsQualitativeResultPassed = inspectionObject.IsQualitativeResultPassed;
                resultObject.QuantitativeResult = inspectionObject.QuantitativeResult;
                resultObject.IsQuantitativeResultPassed = inspectionObject.IsQuantitativeResultPassed;
                resultObject.Remarks = inspectionObject.Remarks;
                await _uow.purchase_qc_sample_result.UpdateAsync(resultObject, userId, ct);
            }
            await _uow.CommitAsync();
            return true;
        }

        #region Custom FluentValidations
        public async Task<bool> IsPurchaseQcIdAvailable(string? Id, CancellationToken ct)
        {
            var result = await _uow.purchase_qc.AnyAsync(ct, x => x.Id == Id);
            return result.Data;
        }
        #endregion Custom FluentValidations
    }
}
