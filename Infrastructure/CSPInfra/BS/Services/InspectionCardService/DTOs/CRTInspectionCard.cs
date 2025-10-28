using DM.DomainModels;

namespace BS.Services.InspectionCardService.DTOs
{
    public static class CRTInspectionCard
    {
        #region Add Inspection Card with Characterstics
        public static Inspection_Card ToDomain(this AddInspectionCardWithCharacteristicsDTO request, string userId)
        {
            return new Inspection_Card()
            {
                Description = request.Description,

                Id = Guid.NewGuid().ToString(),
                IsActive = request.IsActive,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false
            };
        }
        public static Inspection_Characteristic_Mapping ToDomain(this AddInspectionCardWithCharacteristicsDTO request, string userId, string pCharactersticId, string pInspectionCardId)
        {
            return new Inspection_Characteristic_Mapping()
            {
                InspectionCardId = pInspectionCardId,
                CharacteristicId = pCharactersticId,

                Id = Guid.NewGuid().ToString(),
                IsActive = request.IsActive,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false
            };
        }
        #endregion Attach Characterstics to Card
        
        #region List All Characteristics By Inspection Card Id
        public static ResponseCardWithCharacteristics ToResponse(this Inspection_Card row, bool isOverloaded)
        {
            return new ResponseCardWithCharacteristics()
            {
                Id = row.Id,
                IntCode = row.IntCode,
                Description = row.Description,
                CreatedDate = row.CreatedDate,
                CreatedBy = row.CreatedBy,
                UpdatedDate = row.UpdatedDate,
                UpdatedBy = row.UpdatedBy,
                IsActive = row.IsActive,
                IsArchived = row.IsArchived,
                InspectionCharacteristicResults = row.Inspection_Characteristic_Mappings.Select(x => new InspectionCharacteristicResults
                {
                    Id = x.Inspection_Characteristic?.Id,
                    CreatedBy = x.Inspection_Characteristic?.CreatedBy,
                    CreatedDate = x.Inspection_Characteristic?.CreatedDate ?? DateTime.MinValue,
                    UpdatedBy = x.Inspection_Characteristic?.UpdatedBy,
                    UpdatedDate = x.Inspection_Characteristic?.UpdatedDate ?? DateTime.MinValue,
                    IsActive = x.Inspection_Characteristic?.IsActive ?? false,
                    IsArchived = x.Inspection_Characteristic?.IsArchived ?? false,

                    Description = x.Inspection_Characteristic?.Description,
                    Type = x.Inspection_Characteristic?.Type,
                    SingleCriteria = x.Inspection_Characteristic?.SingleCriteria,
                    QuantitativeCriteria = x.Inspection_Characteristic?.QuantitativeCriteria,
                    IntCode = x.Inspection_Characteristic?.IntCode,
                    AttributeId = x.Inspection_Characteristic?.AttributeId,
                    Attribute = new AttributeInCardResponse()
                    {
                        IntCode = x.Inspection_Characteristic?.Inspection_Attribute?.IntCode ?? 0,
                        Name = x.Inspection_Characteristic?.Inspection_Attribute?.Name,
                        Description = x.Inspection_Characteristic?.Inspection_Attribute?.Description,
                        Tag = x.Inspection_Characteristic?.Inspection_Attribute?.Tag,

                        CreatedBy = x.Inspection_Characteristic?.Inspection_Attribute?.CreatedBy,
                        CreatedDate = x.Inspection_Characteristic?.Inspection_Attribute?.CreatedDate ?? DateTime.MinValue,
                        UpdatedBy = x.Inspection_Characteristic?.Inspection_Attribute?.UpdatedBy,
                        UpdatedDate = x.Inspection_Characteristic?.Inspection_Attribute?.UpdatedDate ?? DateTime.MinValue,
                        IsActive = x.Inspection_Characteristic?.Inspection_Attribute?.IsActive ?? false,
                        IsArchived = x.Inspection_Characteristic?.Inspection_Attribute?.IsArchived ?? false,
                    },
                }).ToList()
            };
        }
        #endregion List All Characteristics By Inspection Card Id

        #region CharacteristicWithCriteriaInResponse
        public static CharacteristicWithCriteriaInResponse ToResponseWithCriteria(this Inspection_Characteristic? ch)
        {
            return new CharacteristicWithCriteriaInResponse
            {
                Id = ch.Id,
                CreatedBy = ch.CreatedBy,
                CreatedDate = ch.CreatedDate,
                UpdatedBy = ch.UpdatedBy,
                UpdatedDate = ch.UpdatedDate,
                IsActive = ch.IsActive,
                IsArchived = ch.IsArchived,

                IntCode = ch.IntCode,
                AttributeId = ch.AttributeId,
                Description = ch.Description,
                Type = ch.Type,
                SingleCriteria = ch.SingleCriteria,
                QuantitativeCriteria = ch.QuantitativeCriteria,
                Attribute = new AttributeInCharacteristicResponse()
                {
                    Id = ch.Inspection_Attribute?.Id ?? string.Empty,
                    IntCode = ch.Inspection_Attribute?.IntCode ?? 0,
                    Name = ch.Inspection_Attribute?.Name,
                    Description = ch.Inspection_Attribute?.Description,
                    Tag = ch.Inspection_Attribute?.Tag,

                    CreatedBy = ch.Inspection_Attribute?.CreatedBy,
                    CreatedDate = ch.Inspection_Attribute?.CreatedDate ?? DateTime.MinValue,
                    UpdatedBy = ch.Inspection_Attribute?.UpdatedBy,
                    UpdatedDate = ch.Inspection_Attribute?.UpdatedDate ?? DateTime.MinValue,
                    IsActive = ch.Inspection_Attribute?.IsActive ?? false,
                    IsArchived = ch.Inspection_Attribute?.IsArchived ?? false,
                },
            };
        }
        #endregion CharacteristicWithCriteriaInResponse

        #region List Inspection Card
        public static ResponseInspectionCard ToResponse(this Inspection_Card row)
        {
            return new ResponseInspectionCard()
            {
                Description = row.Description,
                IntCode = row.IntCode,

                Id = row.Id,
                IsActive = row.IsActive,
                CreatedDate = row.CreatedDate,
                CreatedBy = row.CreatedBy,
                UpdatedBy = row.UpdatedBy,
                UpdatedDate = row.UpdatedDate,
                IsArchived = row.IsArchived
            };
        }

        public static List<ResponseInspectionCard> ToResponseList(this IEnumerable<Inspection_Card> rows)
        {
            return rows.Select(x => x.ToResponse()).ToList();
        }
        #endregion List Inspection Card

        #region Update Inspection Card
        public static Inspection_Characteristic_Mapping ToDomain(this UpdateInspectionCardDTO request, string pCharactersticId, string userId)
        {
            return new Inspection_Characteristic_Mapping()
            {
                InspectionCardId = request.Id,
                CharacteristicId = pCharactersticId,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }
        #endregion Update Inspection Card
    }
}
