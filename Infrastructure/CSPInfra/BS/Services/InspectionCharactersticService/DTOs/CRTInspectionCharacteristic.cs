using BS.EnumsAndConstants.Constant;
using DM.DomainModels;
namespace BS.Services.InspectionCharactersticService.DTOs
{
    public static class CRTInspectionCharacteristic
    {
        #region Add Characterstic With Criteria
        public static Inspection_Characteristic ToDomain(this AddCharacteristicWithCriteriaDTO request, string userId)
        {
            return new Inspection_Characteristic()
            {
                AttributeId = request.AttributeId,
                Description = request.Description,
                Type = request.Type,
                SingleCriteria = request.SingleCriteria,
                QuantitativeCriteria = request.QuantitativeCriteria,

                Id = Guid.NewGuid().ToString(),
                IsActive = request.IsActive,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false
            };
        }

        public static Inspection_Characteristic ToDomainQualitative(this AddCharacteristicWithCriteriaDTO request, string userId)
        {
            return new Inspection_Characteristic()
            {
                AttributeId = request.AttributeId,
                Description = request.Description,
                Type = KConstantInspectionTypeBS.qualitative,
                SingleCriteria = request.SingleCriteria,

                Id = Guid.NewGuid().ToString(),
                IsActive = request.IsActive,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false
            };
        }

        public static Inspection_Characteristic ToDomainQuantitative(this AddCharacteristicWithCriteriaDTO request, string userId)
        {
            return new Inspection_Characteristic()
            {
                AttributeId = request.AttributeId,
                Description = request.Description,
                Type = KConstantInspectionTypeBS.quantitative,
                QuantitativeCriteria = request.QuantitativeCriteria,

                Id = Guid.NewGuid().ToString(),
                IsActive = request.IsActive,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false
            };
        }
        #endregion Add Characterstic With Criteria

        #region List All Inspection Characterstics
        public static ResponseInspectionCharacteristic ToResponse(this Inspection_Characteristic row)
        {
            return new ResponseInspectionCharacteristic()
            {
                Description= row.Description,
                Type = row.Type,
                SingleCriteria = row.SingleCriteria,
                QuantitativeCriteria = row.QuantitativeCriteria,
                AttributeId = row.AttributeId,

                Id = row.Id,
                IntCode = row.IntCode,
                IsActive = row.IsActive,
                CreatedDate = row.CreatedDate,
                CreatedBy = row.CreatedBy,
                UpdatedBy = row.UpdatedBy,
                UpdatedDate = row.UpdatedDate,
                IsArchived = row.IsArchived,
                Attribute = new AttributeInInspectionCharacteristic()
                {
                    Id = row.Inspection_Attribute?.Id ?? string.Empty,
                    IntCode = row.Inspection_Attribute?.IntCode ?? 0,
                    Name = row.Inspection_Attribute?.Name,
                    Description = row.Inspection_Attribute?.Description,
                    Tag = row.Inspection_Attribute?.Tag,

                    CreatedBy = row .Inspection_Attribute?.CreatedBy,
                    CreatedDate = row.Inspection_Attribute?.CreatedDate ?? DateTime.MinValue,
                    UpdatedBy = row .Inspection_Attribute?.UpdatedBy,
                    UpdatedDate = row.Inspection_Attribute?.UpdatedDate ?? DateTime.MinValue,
                    IsActive = row.Inspection_Attribute?.IsActive ?? false,
                    IsArchived = row.Inspection_Attribute?.IsArchived ?? false,
                },
            };
        }

        public static List<ResponseInspectionCharacteristic> ToResponseList(this IEnumerable<Inspection_Characteristic> rows)
        {
            return rows.Select(x => x.ToResponse()).ToList();
        }
        #endregion List All Inspection Characterstics

        #region List Characteristics With Criteria
        public static ResponseListCharacteristicsWithCriteria ToResponseCharacteristic(this Inspection_Characteristic x)
        {
            return new ResponseListCharacteristicsWithCriteria
            {
                Id = x.Id,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                IsArchived = x.IsArchived,

                IntCode = x.IntCode,
                Description = x.Description,
                Type = x.Type,
                SingleCriteria = x.SingleCriteria,
                QuantitativeCriteria = x.QuantitativeCriteria,
                AttributeId = x.AttributeId,
                AttributeDescription = x.Inspection_Attribute?.Description,
                Attribute = new AttributeInCharacteristicsWithCriteria()
                {
                    Id = x.Inspection_Attribute?.Id ?? string.Empty,
                    IntCode = x.Inspection_Attribute?.IntCode ?? 0,
                    Name = x.Inspection_Attribute?.Name,
                    Description = x.Inspection_Attribute?.Description,
                    Tag = x.Inspection_Attribute?.Tag,

                    CreatedBy = x.Inspection_Attribute?.CreatedBy,
                    CreatedDate = x.Inspection_Attribute?.CreatedDate ?? DateTime.MinValue,
                    UpdatedBy = x.Inspection_Attribute?.UpdatedBy,
                    UpdatedDate = x.Inspection_Attribute?.UpdatedDate ?? DateTime.MinValue,
                    IsActive = x.Inspection_Attribute?.IsActive ?? false,
                    IsArchived = x.Inspection_Attribute?.IsArchived ?? false,
                }
            };
        }
        public static List<ResponseListCharacteristicsWithCriteria> ToResponseListCharacteristics(this IEnumerable<Inspection_Characteristic> rows)
        {
            return rows.Select(x => x.ToResponseCharacteristic()).ToList();
        }
        #endregion List Characteristics With Criteria
    }
}
