using DM.DomainModels;

namespace BS.Services.ProductionQACavitySampleService.DTOs
{
    public static class CRTProductionQACavitySample
    {
        #region Add
        public static Production_QA_Cavity_Sample ToDomain(this AddProductionQACavitySampleDTO request, string userId)
        {
            return new Production_QA_Cavity_Sample
            {
                Id = Guid.NewGuid().ToString(),
                CavityId = request.CavityId,
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

        public static Production_QA_Cavity_Sample_Result ToDomain(this InspectionContract obj, string userId, string sampleId)
        {
            return new Production_QA_Cavity_Sample_Result
            {
                Id = Guid.NewGuid().ToString(),
                QaCavitySampleId = sampleId,
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

        public static Log_Production_QA_Cavity_Sample_Result ToDomain(this Production_QA_Cavity_Sample_Result resultObject, string userId, bool? pIsSamplePassed)
        {
            return new Log_Production_QA_Cavity_Sample_Result()
            {
                QaCavitySampleResultId = resultObject.Id,
                QaCavitySampleId = resultObject.QaCavitySampleId,
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
        #endregion Add

        #region List
        public static ResponseProductionQACavitySample ToResponseCombined(this Production_QA_Cavity_Sample sample)
        {
            return new ResponseProductionQACavitySample
            {
                Id = sample.Id,
                CreatedBy = sample.CreatedBy,
                CreatedDate = sample.CreatedDate,
                UpdatedBy = sample.UpdatedBy,
                UpdatedDate = sample.UpdatedDate,
                IsArchived = sample.IsArchived,
                IsActive = sample.IsActive,

                CavityId = sample.CavityId,
                InspectionDateTime = sample.InspectionDateTime,
                InspectionBy = sample.InspectionBy,
                InspectionQuantity = sample.InspectionQuantity,
                IsSamplePassed = sample.IsSamplePassed,
                InspectionObjects = sample.Production_QA_Cavity_Sample_Results.Select(r => new InspectionInfo
                {
                    Id = r.Id,
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    UpdatedBy = r.UpdatedBy,
                    UpdatedDate = r.UpdatedDate,
                    IsArchived = r.IsArchived,
                    IsActive = r.IsActive,

                    QaCavitySampleId = r.QaCavitySampleId,
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

        public static List<ResponseProductionQACavitySample> ToResponseList(this IEnumerable<Production_QA_Cavity_Sample> samples)
        {
            return samples.Select(x => x.ToResponseCombined()).ToList();
        }
        #endregion
    }
}
