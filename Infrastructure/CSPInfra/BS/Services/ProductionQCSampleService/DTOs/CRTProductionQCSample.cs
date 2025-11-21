using DM.DomainModels;

namespace BS.Services.ProductionQCSampleService.DTOs
{
    public static class CRTProductionQCSample
    {
        #region Add
        public static Production_QC_Sample ToDomain(this AddProductionQCSampleDTO request, string userId)
        {
            return new Production_QC_Sample
            {
                Id = Guid.NewGuid().ToString(),
                QcId = request.QcId,
                Name = request.Name,
                InspectionDateTime = request.InspectionDateTime,
                InspectionBy = request.InspectionBy,
                InspectionQuantity = request.InspectionQuantity,
                IsSamplePassed = request.IsSamplePassed,

                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }

        public static Production_QC_Sample_Result ToDomain(this InspectionObject obj, string userId, string sampleId)
        {
            return new Production_QC_Sample_Result
            {
                Id = Guid.NewGuid().ToString(),
                QcSampleId = sampleId,
                QualitativeInspectionMappingId = obj.QualitativeInspectionMappingId,
                QuantitativeInspectionMappingId = obj.QuantitativeInspectionMappingId,
                QualitativeResultId = obj.QualitativeResultId,
                IsQualitativeResultPassed = obj.IsQualitativeResultPassed,
                QuantitativeResult = obj.QuantitativeResult,
                IsQuantitativeResultPassed = obj.IsQuantitativeResultPassed,
                Remarks = obj.Remarks,

                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }
        #endregion Add

        #region List
        public static ResponseProductionQCSample ToResponseCombined(this Production_QC_Sample sample)
        {
            return new ResponseProductionQCSample
            {
                Id = sample.Id,
                IntCode = sample.IntCode,
                CreatedBy = sample.CreatedBy,
                CreatedDate = sample.CreatedDate,
                UpdatedBy = sample.UpdatedBy,
                UpdatedDate = sample.UpdatedDate,
                IsArchived = sample.IsArchived,
                IsActive = sample.IsActive,

                QcId = sample.QcId,
                InspectionDateTime = sample.InspectionDateTime,
                InspectionBy = sample.InspectionBy,
                InspectionQuantity = sample.InspectionQuantity,
                IsSamplePassed = sample.IsSamplePassed,
                InspectionObjects = sample.Production_QC_Sample_Results.Select(r => new InspectionResult
                    {
                        Id = r.Id,
                        CreatedBy = r.CreatedBy,
                        CreatedDate = r.CreatedDate,
                        UpdatedBy = r.UpdatedBy,
                        UpdatedDate = r.UpdatedDate,
                        IsArchived = r.IsArchived,
                        IsActive = r.IsActive,

                        QcSampleId = r.QcSampleId,
                        QualitativeInspectionMappingId = r.QualitativeInspectionMappingId,
                        QuantitativeInspectionMappingId = r.QuantitativeInspectionMappingId,
                        QualitativeResultId = r.QualitativeResultId,
                        IsQualitativeResultPassed = r.IsQualitativeResultPassed,
                        QuantitativeResult = r.QuantitativeResult,
                        IsQuantitativeResultPassed = r.IsQuantitativeResultPassed,
                        Remarks = r.Remarks
                    }).ToList()
            };
        }

        public static List<ResponseProductionQCSample> ToResponseList(this IEnumerable<Production_QC_Sample> samples)
        {
            return samples.Select(x => x.ToResponseCombined()).ToList();
        }
        #endregion List

        public static Log_Production_QC_Sample_Result ToDomain(this Production_QC_Sample_Result resultObject, string userId, bool? pIsSamplePassed)
        {
            return new Log_Production_QC_Sample_Result()
            {
                QcSampleResultId = resultObject.Id,
                QcSampleId = resultObject.QcSampleId,
                QualitativeInspectionMappingId = resultObject.QualitativeInspectionMappingId,
                QualitativeResultId = resultObject.QualitativeResultId,
                IsQualitativeResultPassed = resultObject.IsQualitativeResultPassed,
                QuantitativeInspectionMappingId = resultObject.QuantitativeInspectionMappingId,
                QuantitativeResult = resultObject.QuantitativeResult,
                IsQuantitativeResultPassed = resultObject.IsQuantitativeResultPassed,
                Remarks = resultObject.Remarks,
                IsSamplePassed = pIsSamplePassed,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = resultObject.IsActive,
                IsArchived = false
            };
        }

    }
}