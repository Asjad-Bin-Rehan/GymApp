using BS.Services.ProductionQACavitySampleService.DTOs;
using DA;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.ProductionQACavitySampleService
{
    public class ProductionQACavitySampleService : IProductionQACavitySampleService
    {
        private readonly IUnitOfWork _uow;
        public ProductionQACavitySampleService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> AddProductionQACavitySample(AddProductionQACavitySampleDTO request, string userId, CancellationToken ct)
        {
            var sample = request.ToDomain(userId);
            await _uow.production_qa_cavity_sample.AddAsync(sample, userId, ct);

            if (request.InspectionObjects != null && request.InspectionObjects.Any())
            {
                foreach (var inspectionObject in request.InspectionObjects)
                {
                    var sampleResult = inspectionObject.ToDomain(userId, sample.Id);
                    await _uow.production_qa_cavity_sample_result.AddAsync(sampleResult, userId, ct);

                    var logResult = sampleResult.ToDomain(userId, sample.IsSamplePassed);
                    await _uow.log_production_qa_cavity_sample_result.AddAsync(logResult, userId, ct);
                }
            }

            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponseProductionQACavitySample>> ListAllProductionQACavitySamplesByCavityId(string cavityId, CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.production_qa_cavity_sample.GetQueryable();

            var result = await query.Data
                                    .Where(x => x.CavityId == cavityId)
                                    .Include(x => x.Production_QA_Cavity_Sample_Results)
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Skip(skipRecords)
                                    .Take(lastCount)
                                    .ToListAsync(ct);

            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            return result.ToResponseList();
        }

        public async Task<List<ResponseProductionQACavitySample>> GetProductionQACavitySampleById(CancellationToken ct, string sampleId)
        {
            var query = _uow.production_qa_cavity_sample.GetQueryable();
            var result = await query.Data
                                    .Include(x => x.Production_QA_Cavity_Sample_Results)
                                    .Where(x => x.Id == sampleId)
                                    .OrderByDescending(x => x.CreatedDate)
                                    .ToListAsync(ct);

            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            return result.ToResponseList();
        }

        public async Task<bool> UpdateProductionQACavitySample(UpdateProductionQACavitySampleDTO request, string userId, CancellationToken ct)
        {
            // Fetch the Sample
            var query = _uow.production_qa_cavity_sample.GetQueryable();
            var result = await query.Data.Include(x => x.Production_QA_Cavity_Sample_Results).Where(x => x.Id == request.Id).SingleOrDefaultAsync(ct) ?? throw new ArgumentFalseException("No active record found to update");
            var existingSamplePassStatus = result.IsSamplePassed;

            // Update the Header Fields
            result.Name = request.Name;
            result.IsSamplePassed = request.IsSamplePassed;
            result.InspectionDateTime = request.InspectionDateTime;
            result.InspectionBy = request.InspectionBy;
            result.IsActive = request.IsActive;
            await _uow.production_qa_cavity_sample.UpdateAsync(result, userId, ct);

            foreach (var inspectionObject in request.InspectionObjects)
            {
                // Fetch the Sample Results
                var resultObject = result.Production_QA_Cavity_Sample_Results.FirstOrDefault(x =>
                    x.QaCavitySampleId == request.Id
                    && x.QualitativeInspectionMappingId == inspectionObject.QualitativeInspectionMappingId
                    && x.QuantitativeInspectionMappingId == inspectionObject.QuantitativeInspectionMappingId
                );

                // Log the Sample Results & Pass-Status Header Field
                var logResultObject = resultObject?.ToDomain(userId, existingSamplePassStatus);
                await _uow.log_production_qa_cavity_sample_result.AddAsync(logResultObject, userId, ct);

                // Update the Sample Results
                resultObject.QualitativeResultId = inspectionObject.QualitativeResultId;
                resultObject.IsQualitativeResultPassed = inspectionObject.IsQualitativeResultPassed;
                resultObject.QuantitativeResult = inspectionObject.QuantitativeResult;
                resultObject.IsQuantitativeResultPassed = inspectionObject.IsQuantitativeResultPassed;
                resultObject.Remarks = inspectionObject.Remarks;
                await _uow.production_qa_cavity_sample_result.UpdateAsync(resultObject, userId, ct);
            }
            await _uow.CommitAsync();
            return true;
        }

        #region Custom FluentValidations
        public async Task<bool> IsCavityIdAvailable(string? Id, CancellationToken ct)
        {
            var result = await _uow.production_qa_cavity.AnyAsync(ct, x => x.Id == Id);
            return result.Data;
        }
        #endregion Custom FluentValidations
    }
}
