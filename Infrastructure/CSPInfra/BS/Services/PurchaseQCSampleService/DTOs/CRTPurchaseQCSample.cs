using DM.DomainModels;

namespace BS.Services.PurchaseQCSampleService.DTOs
{
    public static class CRTPurchaseQCSample
    {
        public static Purchase_QC_Sample ToDomain(this AddPurchaseQCSampleDTO request, string userId)
        {
            return new Purchase_QC_Sample()
            {
                InspectionDateTime = request.InspectionDateTime,
                InspectionBy = request.InspectionBy,
                QcId = request.QcId,
                Name = request.Name,
                IsSamplePassed = request.IsSamplePassed,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }


        public static Purchase_QC_Sample_Result ToDomain(this InspectionObjects inspection, string userId, string qcSampleId)
        {
            return new Purchase_QC_Sample_Result()
            {
                QcSampleId = qcSampleId,
                QualitativeInspectionMappingId = inspection.QualitativeInspectionMappingId,
                QuantitativeInspectionMappingId = inspection.QuantitativeInspectionMappingId,
                QualitativeResultId = inspection.QualitativeResultId,
                IsQualitativeResultPassed = inspection.IsQualitativeResultPassed,
                QuantitativeResult = inspection.QuantitativeResult,
                IsQuantitativeResultPassed = inspection.IsQuantitativeResultPassed,
                Remarks = inspection.Remarks,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }


        public static ResponsePurchaseQCSampleDTO ToResponse(this Purchase_QC_Sample sample)
        {
            return new ResponsePurchaseQCSampleDTO
            {
                Id = sample.Id,
                CreatedBy = sample.CreatedBy,
                CreatedDate = sample.CreatedDate,
                UpdatedBy = sample.UpdatedBy,
                UpdatedDate = sample.UpdatedDate,
                IsActive = sample.IsActive,
                IsArchived = sample.IsArchived,

                IntCode = sample.IntCode,
                QcId = sample.QcId,
                Name = sample.Name,
                InspectionDateTime = sample.InspectionDateTime,
                InspectionBy = sample.InspectionBy,
                InspectionQuantity = sample.InspectionQuantity,
                IsSamplePassed = sample.IsSamplePassed,
                InspectionObjects = sample.Purchase_QC_Sample_Results.Select(r => new InspectionResults
                    {
                        Id = r.Id,
                        CreatedBy = r.CreatedBy,
                        CreatedDate = r.CreatedDate,
                        UpdatedBy = r.UpdatedBy,
                        UpdatedDate = r.UpdatedDate,
                        IsActive = r.IsActive,
                        IsArchived = r.IsArchived,

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

        public static List<ResponsePurchaseQCSampleDTO> ToResponseList(this IEnumerable<Purchase_QC_Sample> samples)
        {
            return samples.Select(x => x.ToResponse()).ToList();
        }

        public static Log_Purchase_QC_Sample_Result ToDomain(this Purchase_QC_Sample_Result resultObject, string userId, bool? pIsSamplePassed)
        {
            return new Log_Purchase_QC_Sample_Result()
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
