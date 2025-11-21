using DM.DomainModels;

namespace BS.Services.ItemInspectionCardService.DTOs
{
    public static class CRTItemInspectionCard
    {
        #region Add Item Inspection Card With Both Inspections
        public static Item ToDomain(this AddItemInspectionCardWithBothInspectionsDTO request, string userId)
        {
            return new Item()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.ItemName,
                Type = request.ItemType,
                GroupCode = request.ItemGroupCode,
                U_QACard = request.ItemU_QACard,
                UoMGroupEntry = request.ItemUoMGroupEntry,
                ItemCode = request.ItemCode,
                IsBatch = request.IsBatch,
                IsEnabledForQA = request.IsEnabledForQA,
                ManageBatchNumbers = request.ManageBatchNumbers,
                GroupName = request.GroupName,
                PackSize = request.PackSize,

                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }

        public static Item_Inspection_Card ToDomain(this AddItemInspectionCardWithBothInspectionsDTO request, string userId, string itemId)
        {
            return new Item_Inspection_Card()
            {
                Id = Guid.NewGuid().ToString(),
                ItemId = itemId,
                InspectionCardId = request.InspectionCardId,
                ItemDescription = request.ItemDescription,
                CardDescription = request.CardDescription,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }

        public static Qualitative_Inspection ToDomainQualitative(this AddItemInspectionCardWithBothInspectionsDTO request, string userId, string inspectionCardId, bool overloaded)
        {
            return new Qualitative_Inspection()
            {
                Id = Guid.NewGuid().ToString(),
                ItemInspectionCardId = inspectionCardId,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }

        public static Quantitative_Inspection ToDomainQuantitative(this AddItemInspectionCardWithBothInspectionsDTO request, string userId, string inspectionCardId, bool overloaded)
        {
            return new Quantitative_Inspection()
            {
                Id = Guid.NewGuid().ToString(),
                ItemInspectionCardId = inspectionCardId,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }

        public static Qualitative_Inspection_Mapping ToDomain(this QualitativeInspectionContract contract, string userId, string qualitativeInspectionId)
        {
            return new Qualitative_Inspection_Mapping()
            {
                QualitativeInspectionId = qualitativeInspectionId,
                CharacteristicId = contract.InspectionCharacteristicId,
                IsMandatory = contract.IsMandatory,
                IsDispatch = contract.IsDispatch,
                IsIncoming = contract.IsIncoming,
                IsQcCritical = contract.IsQcCritical,
                IsQcFloor = contract.IsQcFloor,
                IsQcLab = contract.IsQcLab,
                IsTrial = contract.IsTrial,
                QualitativeSpec = contract.QualitativeSpec,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }

        public static Qualitative_Result_Pass_Status ToDomain(this QualitativeResultPassStatusContract contract, string userId, string mappingId)
        {
            return new Qualitative_Result_Pass_Status()
            {
                Id = Guid.NewGuid().ToString(),
                QualitativeInspectionMappingId = mappingId,
                QualitativeResultId = contract.QualitativeResultId,
                IsPassed = contract.IsPassed,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }

        public static Quantitative_Inspection_Mapping ToDomain(this QuantitativeInspectionContract contract, string userId, string quantitativeInspectionId)
        {
            return new Quantitative_Inspection_Mapping()
            {
                QuantitativeInspectionId = quantitativeInspectionId,
                CharacteristicId = contract.InspectionCharacteristicId,
                UoMId = contract.UoMId,
                IsMandatory = contract.IsMandatory,
                IsIncoming = contract.IsIncoming,
                IsQcCritical = contract.IsQcCritical,
                IsQcFloor = contract.IsQcFloor,
                IsQcLab = contract.IsQcLab,
                IsTrial = contract.IsTrial,
                QuantitativeSpec = contract.QuantitativeSpec,
                Target = contract.Target,
                Max = contract.Max,
                Min = contract.Min,
                UpperLimit = contract.UpperLimit,
                LowerLimit = contract.LowerLimit,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
        }
        #endregion Add Item Inspection Card With Both Inspections

        #region List All Item Inspection Cards
        public static ResponseItemInspectionCard ToResponse(this Item_Inspection_Card row)
        {
            return new ResponseItemInspectionCard()
            {
                CardDescription = row.CardDescription,
                ItemDescription = row.ItemDescription,
                ItemId = row.ItemId,
                InspectionCardId = row.InspectionCardId,
                IntCode = row.IntCode,

                Id = row.Id,
                IsActive = row.IsActive,
                CreatedBy = row.CreatedBy,
                CreatedDate = row.CreatedDate,
                UpdatedBy = row.UpdatedBy,
                UpdatedDate = row.UpdatedDate,
                IsArchived = row.IsArchived
            };
        }

        public static List<ResponseItemInspectionCard> ToResponseList(this IEnumerable<Item_Inspection_Card> rows)
        {
            return rows.Select(x => x.ToResponse()).ToList();
        }
        #endregion List All Item Inspection Cards

        #region List Cards By Both Characteristics
        public static ResponseCardWithBothCharacteristicsDTO ToResponseCombined(this Item_Inspection_Card card, bool isOverloaded, bool isOverloadedAgain)
        {
            return new ResponseCardWithBothCharacteristicsDTO
            {
                Id = card.Id,
                IntCode = card.IntCode,
                Type = card.Type,
                CardDescription = card.CardDescription,
                InspectionCardIntCode = card.Inspection_Card?.IntCode ?? 0,
                ItemDescription = card.ItemDescription,
                ItemId = card.ItemId,
                InspectionCardId = card.InspectionCardId,

                CreatedBy = card.CreatedBy,
                CreatedDate = card.CreatedDate,
                UpdatedBy = card.UpdatedBy,
                UpdatedDate = card.UpdatedDate,
                IsActive = card.IsActive,
                IsArchived = card.IsArchived,

                QualitativeInspectionObjects = card.Qualitative_Inspection?.Qualitative_Inspection_Mappings
                    .Select(qim => new QualitativeInspectionDisplay
                    {
                        Id = qim.CharacteristicId,
                        CreatedBy = qim.CreatedBy,
                        CreatedDate = qim.CreatedDate,
                        UpdatedBy = qim.UpdatedBy,
                        UpdatedDate = qim.UpdatedDate,
                        IsActive = qim.IsActive,
                        IsArchived = qim.IsArchived,

                        QualitativeInspectionId = qim.QualitativeInspectionId,
                        InspectionCharacterisicMappingId = qim.Id,
                        InspectionCharacteristicName = qim.Inspection_Characteristic?.Description,
                        InspectionCharacteristicSingleCriteria = qim.Inspection_Characteristic?.SingleCriteria,
                        InspectionCharacteristicQuantitativeCriteria = qim.Inspection_Characteristic?.QuantitativeCriteria,
                        IsMandatory = qim.IsMandatory ?? false,
                        IsTrial = qim.IsTrial ?? false,
                        QualitativeSpec = qim.QualitativeSpec,
                        IsQcLab = qim.IsQcLab ?? false,
                        IsQcFloor = qim.IsQcFloor ?? false,
                        IsQcCritical = qim.IsQcCritical ?? false,
                        IsIncoming = qim.IsIncoming ?? false,
                        IsDispatch = qim.IsDispatch ?? false,
                        Attribute = new AttributeInIIC()
                        {
                            Id = qim.Inspection_Characteristic?.Inspection_Attribute?.Id ?? string.Empty,
                            IntCode = qim.Inspection_Characteristic?.Inspection_Attribute?.IntCode ?? 0,
                            Name = qim.Inspection_Characteristic?.Inspection_Attribute?.Name,
                            IsActive = qim.Inspection_Characteristic?.Inspection_Attribute?.IsActive ?? false,
                        },
                        QualitativeResultPassStatusResults = qim.Qualitative_Result_Pass_Statuses?
                            .Select(qrp => new QualitativeResultPassStatusDisplay
                            {
                                PassStatusId = qrp.Id,
                                QualitativeResultId = qrp.QualitativeResultId,
                                ResultDescription = qrp.Qualitative_Result?.ResultDescription,
                                IsPassed = qrp.IsPassed,
                                IsActive = qrp.IsActive,
                            }).ToList() ?? [],
                    }).ToList() ?? [],

                QuantitativeInspectionResults = card.Quantitative_Inspection?.Quantitative_Inspection_Mappings
                    .Select(qim => new QuantitativeInspectionDisplay
                    {
                        Id = qim.CharacteristicId,
                        CreatedBy = qim.CreatedBy,
                        CreatedDate = qim.CreatedDate,
                        UpdatedBy = qim.UpdatedBy,
                        UpdatedDate = qim.UpdatedDate,
                        IsActive = qim.IsActive,
                        IsArchived = qim.IsArchived,
                        
                        QuantitativeInspectionId = qim.QuantitativeInspectionId,
                        InspectionCharacterisicMappingId = qim.Id,
                        InspectionCharacteristicName = qim.Inspection_Characteristic?.Description,
                        IsMandatory = qim.IsMandatory ?? false,
                        IsDispatch = qim.IsDispatch ?? false,
                        IsIncoming = qim.IsIncoming ?? false,
                        IsQcCritical = qim.IsQcCritical ?? false,
                        IsQcFloor = qim.IsQcFloor ?? false,
                        IsQcLab = qim.IsQcLab ?? false,
                        IsTrial = qim.IsTrial ?? false,
                        QuantitativeSpec = qim.QuantitativeSpec,
                        UoMId = qim.UoMId,
                        UoMCode = qim.Unit_Of_Measure?.UoMcode,
                        UoMName = qim.Unit_Of_Measure?.Description,
                        Target = qim.Target,
                        Max = qim.Max,
                        Min = qim.Min,
                        UpperLimit = qim.UpperLimit,
                        LowerLimit = qim.LowerLimit,
                        Attribute = new AttributeInIIC()
                        {
                            Id = qim.Inspection_Characteristic?.Inspection_Attribute?.Id ?? string.Empty,
                            IntCode = qim.Inspection_Characteristic?.Inspection_Attribute?.IntCode ?? 0,
                            Name = qim.Inspection_Characteristic?.Inspection_Attribute?.Name,
                            IsActive = qim.Inspection_Characteristic?.Inspection_Attribute?.IsActive ?? false,
                        }
                    }).ToList() ?? []
            };
        }

        public static List<ResponseCardWithBothCharacteristicsDTO> ToResponseList(this IEnumerable<Item_Inspection_Card> rows, bool isOverloaded, bool isOverloadedAgain)
        {
            return rows.Select(x => x.ToResponseCombined(isOverloaded, isOverloadedAgain)).ToList();
        }
        #endregion List Cards By Both Characteristics
    }
}
