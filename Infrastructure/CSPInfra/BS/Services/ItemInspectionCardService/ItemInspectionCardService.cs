using BS.Services.InspectionCardService.DTOs;
using BS.Services.ItemInspectionCardService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.ItemInspectionCardService
{
    public class ItemInspectionCardService : IItemInspectionCardService
    {
        private IUnitOfWork _uow;
        public ItemInspectionCardService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<bool> AddItemInspectionCardWithBothInspections(AddItemInspectionCardWithBothInspectionsDTO request, string userId, CancellationToken ct)
        {
            Item_Inspection_Card? itemInspectionCard = null;
            Qualitative_Inspection? qualitativeInspection = null;
            Quantitative_Inspection? quantitativeInspection = null;

            var item = request.ToDomain(userId);
            await _uow.item.AddAsync(item, userId, ct);

            itemInspectionCard = request.ToDomain(userId, item.Id);
            await _uow.item_inspection_card.AddAsync(itemInspectionCard, userId, ct);

            #region Create Both Inspections
            if (request.QualitativeInspectionObjects?.Any() == true)
            {
                qualitativeInspection = request.ToDomainQualitative(userId, itemInspectionCard.Id, true);
                await _uow.qualitative_inspection.AddAsync(qualitativeInspection, userId, ct);
            }

            if (request.QuantitativeInspectionObjects?.Any() == true)
            {
                quantitativeInspection = request.ToDomainQuantitative(userId, itemInspectionCard.Id, true);
                await _uow.quantitative_inspection.AddAsync(quantitativeInspection, userId, ct);
            }
            #endregion Create Both Inspections

            #region Create Qualitative Inspection Mappings & Pass Status Results
            if (qualitativeInspection != null && request.QualitativeInspectionObjects != null)
            {
                foreach (var inspectionObject in request.QualitativeInspectionObjects)
                {
                    var qualitativeMapping = inspectionObject.ToDomain(userId, qualitativeInspection.Id);
                    await _uow.qualitative_inspection_mapping.AddAsync(qualitativeMapping, userId, ct);

                    foreach (var passStatusResultObject in inspectionObject.QualitativeResultPassStatusObjects)
                    {
                        var passStatusResult = passStatusResultObject.ToDomain(userId, qualitativeMapping.Id);
                        await _uow.qualitative_result_pass_status.AddAsync(passStatusResult, userId, ct);
                    }
                }
            }
            #endregion Create Qualitative Inspection Mappings & Pass Status Results

            #region Create Quantitative Inspection Mappings
            if (quantitativeInspection != null && request.QuantitativeInspectionObjects != null)
            {
                foreach (var inspectionObjects in request.QuantitativeInspectionObjects)
                {
                    var quantitativeMapping = inspectionObjects.ToDomain(userId, quantitativeInspection.Id);
                    await _uow.quantitative_inspection_mapping.AddAsync(quantitativeMapping, userId, ct);
                }
            }
            #endregion Create Quantitative Inspection Mappings

            await _uow.CommitAsync();
            return true;
        }

        public async Task<ResponseCardWithBothCharacteristicsDTO> GetCardByIdWithBothCharacteristics(string itemInspectionCardId, CancellationToken ct)
        {
            var query = _uow.item_inspection_card.GetQueryable();
            var result = await query.Data
                .Include(x => x.Inspection_Card)
                // Qualitative Inspection includes
                .Include(x => x.Qualitative_Inspection)
                    .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Inspection_Characteristic)
                            .ThenInclude(qim => qim.Inspection_Attribute)
                .Include(x => x.Qualitative_Inspection)
                    .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Qualitative_Result_Pass_Statuses)
                            .ThenInclude(qrp => qrp.Qualitative_Result)
                // Quantitative Inspection includes
                .Include(x => x.Quantitative_Inspection)
                    .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Inspection_Characteristic)
                            .ThenInclude(qim => qim.Inspection_Attribute)
                .Include(x => x.Quantitative_Inspection)
                    .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Unit_Of_Measure)
                .Where(x => x.Id == itemInspectionCardId)
                .SingleOrDefaultAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result != null, "No records found");
            return result.ToResponseCombined(true, true);
        }

        public async Task<List<ResponseCardWithBothCharacteristicsDTO>> ListAllCardsWithBothCharacteristics(CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.item_inspection_card.GetQueryable();
            var result = await query.Data
                .Include(x => x.Inspection_Card)
                // Qualitative Inspection includes
                .Include(x => x.Qualitative_Inspection)
                    .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Inspection_Characteristic)
                            .ThenInclude(qim => qim.Inspection_Attribute)
                .Include(x => x.Qualitative_Inspection)
                    .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Qualitative_Result_Pass_Statuses)
                            .ThenInclude(qrp => qrp.Qualitative_Result)
                // Quantitative Inspection includes
                .Include(x => x.Quantitative_Inspection)
                    .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Inspection_Characteristic)
                            .ThenInclude(qim => qim.Inspection_Attribute)
                .Include(x => x.Quantitative_Inspection)
                    .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Unit_Of_Measure)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(skipRecords)
                .Take(lastCount)
                .ToListAsync(ct);

            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            return result.ToResponseList(true, true);
        }

        public async Task<ResponseCardWithBothCharacteristicsDTO> GetCardByCodeWithBothCharacteristics(string itemCode, CancellationToken ct)
        {
            var query = _uow.item_inspection_card.GetQueryable();
            var result = await query.Data
                .Include(x => x.Inspection_Card)
                // Qualitative Inspection includes
                .Include(x => x.Qualitative_Inspection)
                    .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Inspection_Characteristic)
                            .ThenInclude(qim => qim.Inspection_Attribute)
                .Include(x => x.Qualitative_Inspection)
                    .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Qualitative_Result_Pass_Statuses)
                            .ThenInclude(qrp => qrp.Qualitative_Result)
                // Quantitative Inspection includes
                .Include(x => x.Quantitative_Inspection)
                    .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Inspection_Characteristic)
                            .ThenInclude(qim => qim.Inspection_Attribute)
                .Include(x => x.Quantitative_Inspection)
                    .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Unit_Of_Measure)
                .Where(x => x.Item!=null && x.Item.ItemCode==itemCode)
                .FirstOrDefaultAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result != null, "No records found");
            return result.ToResponseCombined(true, true);
        }

        public async Task<List<ResponseCardWithBothCharacteristicsDTO>> ListCardByCodeWithBothCharacteristics(string itemCode, CancellationToken ct)
        {
            var query = _uow.item_inspection_card.GetQueryable();
            var result = await query.Data
                .Include(x => x.Inspection_Card)
                // Qualitative Inspection includes
                .Include(x => x.Qualitative_Inspection)
                    .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Inspection_Characteristic)
                            .ThenInclude(qim => qim.Inspection_Attribute)
                .Include(x => x.Qualitative_Inspection)
                    .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Qualitative_Result_Pass_Statuses)
                            .ThenInclude(qrp => qrp.Qualitative_Result)
                // Quantitative Inspection includes
                .Include(x => x.Quantitative_Inspection)
                    .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Inspection_Characteristic)
                            .ThenInclude(qim => qim.Inspection_Attribute)
                .Include(x => x.Quantitative_Inspection)
                    .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Unit_Of_Measure)
                .Where(x => x.Item != null && x.Item.ItemCode == itemCode)
                .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result != null, "No records found");
            return result?.ToResponseList(true, true) ?? [];
        }

        public async Task<bool> UpdateItemInspectionCard(UpdateItemInspectionCardDTO request, string userId, CancellationToken ct)
        {
            var itemInspectionCard = await LoadItemInspectionCard(request.Id, ct);
            ArgumentFalseException.ThrowIfFalse(itemInspectionCard!=null, "No records found");
            itemInspectionCard!.IsActive = request.IsActive;
            await _uow.item_inspection_card.UpdateAsync(itemInspectionCard, userId, ct);

            if (itemInspectionCard.Qualitative_Inspection != null && request.QualitativeInspectionResults != null)
            {
                await UpdateQualitativeInspections(_uow, itemInspectionCard.Qualitative_Inspection, request.QualitativeInspectionResults, userId, ct);
            }

            if (itemInspectionCard.Quantitative_Inspection != null && request.QuantitativeInspectionResults != null)
            {
                await UpdateQuantitativeInspections(_uow, itemInspectionCard.Quantitative_Inspection, request.QuantitativeInspectionResults, userId, ct);
            }

            await _uow.CommitAsync();
            return true;
        }

        #region UpdateItemInspectionCardWithBothInspections Helpers
        private async Task<Item_Inspection_Card> LoadItemInspectionCard(string id, CancellationToken ct)
        {
            var query = _uow.item_inspection_card.GetQueryable();
            var existingRecord = await query.Data
                .Include(x => x.Inspection_Card)
                // Include Qualitative Inspection details
                .Include(x => x.Qualitative_Inspection)
                    .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Inspection_Characteristic)
                .Include(x => x.Qualitative_Inspection)
                    .ThenInclude(qi => qi.Qualitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Qualitative_Result_Pass_Statuses)
                            .ThenInclude(qrp => qrp.Qualitative_Result)
                // Include Quantitative Inspection details
                .Include(x => x.Quantitative_Inspection)
                    .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Inspection_Characteristic)
                .Include(x => x.Quantitative_Inspection)
                    .ThenInclude(qi => qi.Quantitative_Inspection_Mappings)
                        .ThenInclude(qim => qim.Unit_Of_Measure)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync(ct);

            ArgumentFalseException.ThrowIfFalse(existingRecord != null, "No records found");
            return existingRecord;
        }

        #region Qualitative Inspection Helpers
        private static async Task UpdateQualitativeInspections(IUnitOfWork uow, Qualitative_Inspection inspection, List<QualitativeInspectionUpdateObject> updates, string userId, CancellationToken ct)
        {
            foreach (var update in updates)
            {
                if (!update.IsActive)
                {
                    await DetachQualitativeInspectionMapping(uow, inspection, update, userId, ct);
                }
                else if (!string.IsNullOrEmpty(update.QualitativeInspectionId))
                {
                    await UpdateExistingQualitativeMapping(uow, inspection, update, userId, ct);
                }
                else
                {
                    await CreateNewQualitativeMapping(uow, inspection, update, userId, ct);
                }
            }
        }

        #region UpdateExisting Helpers
        private static async Task UpdateExistingQualitativeMapping(IUnitOfWork uow, Qualitative_Inspection inspection, QualitativeInspectionUpdateObject update, string userId, CancellationToken ct)
        {
            var existingMapping = inspection.Qualitative_Inspection_Mappings.FirstOrDefault(qim => qim.CharacteristicId == update.Id && qim.QualitativeInspectionId == update.QualitativeInspectionId);
            if (existingMapping != null)
            {
                existingMapping.IsMandatory = update.IsMandatory;
                existingMapping.IsMandatory = update.IsMandatory;
                existingMapping.IsDispatch = update.IsDispatch;
                existingMapping.IsQcCritical = update.IsQcCritical;
                existingMapping.IsTrial = update.IsTrial;
                existingMapping.QualitativeSpec = update.QualitativeSpec;
                existingMapping.IsQcLab = update.IsQcLab;
                existingMapping.IsIncoming = update.IsIncoming;
                existingMapping.IsQcFloor = update.IsQcFloor;
                await uow.qualitative_inspection_mapping.UpdateAsync(existingMapping, userId, ct);
                await UpdateQualitativeResultStatuses(uow, existingMapping, update, userId, ct);
            }
        }

        #region Update QualitativeResultStatus & CreateNew Helpers
        private static async Task UpdateQualitativeResultStatuses(IUnitOfWork uow, Qualitative_Inspection_Mapping mapping, QualitativeInspectionUpdateObject update, string userId, CancellationToken ct)
        {
            if (update.QualitativeResultPassStatusResults != null)
            {
                foreach (var passDto in update.QualitativeResultPassStatusResults)
                {
                    if (!passDto.IsActive)
                    {
                        await DetachQualitativeResultPassStatus(uow, mapping, passDto, userId, ct);
                    }
                    else if (!string.IsNullOrEmpty(passDto.QualitativeResultId))
                    {
                        var existingPass = mapping.Qualitative_Result_Pass_Statuses.FirstOrDefault(ps => ps.QualitativeResultId == passDto.QualitativeResultId);
                        if (existingPass != null)
                        {
                            await UpdateQualitativeResultPassStatus(uow, mapping, passDto, userId, ct);
                        }
                        else
                        {
                            await CreateQualitativeResultPassStatus(uow, mapping, passDto, userId, ct);
                        }
                    }
                }
            }
        }

        private static async Task DetachQualitativeResultPassStatus(IUnitOfWork uow, Qualitative_Inspection_Mapping mapping, QualitativeResultPassStatusUpdateContract passDto, string userId, CancellationToken ct)
        {
            var existingPass = mapping.Qualitative_Result_Pass_Statuses.FirstOrDefault(ps => ps.QualitativeResultId == passDto.QualitativeResultId);
            if (existingPass != null)
            {
                existingPass.IsActive = false;
                await uow.qualitative_result_pass_status.UpdateAsync(existingPass, userId, ct);
            }
        }

        private static async Task UpdateQualitativeResultPassStatus(IUnitOfWork uow, Qualitative_Inspection_Mapping mapping, QualitativeResultPassStatusUpdateContract passDto, string userId, CancellationToken ct)
        {
            var existingPass = mapping.Qualitative_Result_Pass_Statuses.FirstOrDefault(ps => ps.QualitativeResultId == passDto.QualitativeResultId);
            if (existingPass != null)
            {
                existingPass.IsPassed = passDto.IsPassed;
                await uow.qualitative_result_pass_status.UpdateAsync(existingPass, userId, ct);
            }
        }

        private static async Task CreateQualitativeResultPassStatus(IUnitOfWork uow, Qualitative_Inspection_Mapping mapping, QualitativeResultPassStatusUpdateContract passDto, string userId, CancellationToken ct)
        {
            var newPass = new Qualitative_Result_Pass_Status
            {
                Id = Guid.NewGuid().ToString(),
                QualitativeInspectionMappingId = mapping.Id,
                QualitativeResultId = passDto.QualitativeResultId,
                IsPassed = passDto.IsPassed,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };
            await uow.qualitative_result_pass_status.AddAsync(newPass, userId, ct);
        }
        #endregion Update Existing & CreateNew QualitativeResultStatus Helpers

        #endregion UpdateExisting Helpers

        #region CreateNew Helpers
        private static async Task CreateNewQualitativeMapping(IUnitOfWork uow, Qualitative_Inspection inspection, QualitativeInspectionUpdateObject update, string userId, CancellationToken ct)
        {
            var newMapping = new Qualitative_Inspection_Mapping
            {
                Id = Guid.NewGuid().ToString(),
                QualitativeInspectionId = inspection.Id,
                CharacteristicId = update.Id,
                IsMandatory = update.IsMandatory,
                IsTrial = update.IsTrial,
                QualitativeSpec = update.QualitativeSpec,
                IsDispatch = update.IsDispatch,
                IsQcLab = update.IsQcLab,
                IsQcFloor = update.IsQcFloor,
                IsQcCritical = update.IsQcCritical,
                IsIncoming = update.IsIncoming,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };

            inspection.Qualitative_Inspection_Mappings.Add(newMapping);
            await uow.qualitative_inspection_mapping.AddAsync(newMapping, userId, ct);
            await CreateQualitativeResultStatuses(uow, newMapping, update, userId, ct);
        }

        private static async Task CreateQualitativeResultStatuses(IUnitOfWork uow, Qualitative_Inspection_Mapping mapping, QualitativeInspectionUpdateObject update, string userId, CancellationToken ct)
        {
            if (update.QualitativeResultPassStatusResults != null)
            {
                foreach (var passDto in update.QualitativeResultPassStatusResults)
                {
                    var newPass = new Qualitative_Result_Pass_Status
                    {
                        Id = Guid.NewGuid().ToString(),
                        QualitativeInspectionMappingId = mapping.Id,
                        QualitativeResultId = passDto.QualitativeResultId,
                        IsPassed = passDto.IsPassed,
                        CreatedBy = userId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedBy = userId,
                        UpdatedDate = DateTime.UtcNow,
                        IsArchived = false,
                        IsActive = true
                    };
                    await uow.qualitative_result_pass_status.AddAsync(newPass, userId, ct);
                }
            }
        }
        #endregion CreateNew Helpers

        #region Detach Helpers
        private static async Task DetachQualitativeInspectionMapping(IUnitOfWork uow, Qualitative_Inspection inspection, QualitativeInspectionUpdateObject update, string userId, CancellationToken ct)
        {
            var mapping = inspection.Qualitative_Inspection_Mappings.FirstOrDefault(qim => qim.CharacteristicId == update.Id && qim.QualitativeInspectionId == update.QualitativeInspectionId);
            if (mapping != null)
            {
                mapping.IsActive = false;
                await uow.qualitative_inspection_mapping.UpdateAsync(mapping, userId, ct);
            }
        }
        #endregion Detach Helpers

        #endregion Qualitative Inspection Helpers

        #region Quantitative Inspection Helpers
        private static async Task UpdateQuantitativeInspections(IUnitOfWork uow, Quantitative_Inspection inspection, List<QuantitativeInspectionUpdateObject> updates, string userId, CancellationToken ct)
        {
            foreach (var update in updates)
            {
                if (!update.IsActive)
                {
                    await DetachQuantitativeInspectionMapping(uow, inspection, update, userId, ct);
                }
                else if (!string.IsNullOrEmpty(update.QuantitiveInspectionId))
                {
                    await UpdateExistingQuantitativeMapping(uow, inspection, update, userId, ct);
                }
                else
                {
                    await CreateNewQuantitativeMapping(uow, inspection, update, userId, ct);
                }
            }
        }

        private static async Task UpdateExistingQuantitativeMapping(IUnitOfWork uow, Quantitative_Inspection inspection, QuantitativeInspectionUpdateObject update, string userId, CancellationToken ct)
        {
            var existingMapping = inspection.Quantitative_Inspection_Mappings.FirstOrDefault(qim => qim.CharacteristicId == update.Id && qim.QuantitativeInspectionId == update.QuantitiveInspectionId);
            if (existingMapping != null)
            {
                existingMapping.IsMandatory = update.IsMandatory;
                existingMapping.IsDispatch = update.IsDispatch;
                existingMapping.IsTrial = update.IsTrial;
                existingMapping.QuantitativeSpec = update.QuantitativeSpec;
                existingMapping.IsQcCritical = update.IsQcCritical;
                existingMapping.IsQcLab = update.IsQcLab;
                existingMapping.IsIncoming = update.IsIncoming;
                existingMapping.IsQcFloor = update.IsQcFloor;
                existingMapping.Target = update.Target;
                existingMapping.Min = update.Min;
                existingMapping.Max = update.Max;
                existingMapping.UpperLimit = update.UpperLimit;
                existingMapping.LowerLimit = update.LowerLimit;
                existingMapping.UoMId = update.UoMId;
                await uow.quantitative_inspection_mapping.UpdateAsync(existingMapping, userId, ct);
            }
        }

        private static async Task CreateNewQuantitativeMapping(IUnitOfWork uow, Quantitative_Inspection inspection, QuantitativeInspectionUpdateObject update, string userId, CancellationToken ct)
        {
            var newMapping = new Quantitative_Inspection_Mapping
            {
                Id = Guid.NewGuid().ToString(),
                QuantitativeInspectionId = inspection.Id,
                CharacteristicId = update.Id,
                IsMandatory = update.IsMandatory,
                IsTrial = update.IsTrial,
                QuantitativeSpec = update.QuantitativeSpec,
                IsDispatch = update.IsDispatch,
                IsQcLab = update.IsQcLab,
                IsQcFloor = update.IsQcFloor,
                IsQcCritical = update.IsQcCritical,
                IsIncoming = update.IsIncoming,
                Target = update.Target,
                Min = update.Min,
                Max = update.Max,
                UpperLimit = update.UpperLimit,
                LowerLimit = update.LowerLimit,
                UoMId = update.UoMId,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsArchived = false,
                IsActive = true
            };

            inspection.Quantitative_Inspection_Mappings.Add(newMapping);
            await uow.quantitative_inspection_mapping.AddAsync(newMapping, userId, ct);
        }

        private static async Task DetachQuantitativeInspectionMapping(IUnitOfWork uow, Quantitative_Inspection inspection, QuantitativeInspectionUpdateObject update, string userId, CancellationToken ct)
        {
            var mapping = inspection.Quantitative_Inspection_Mappings.FirstOrDefault(qim => qim.CharacteristicId == update.Id && qim.QuantitativeInspectionId == update.QuantitiveInspectionId);
            if (mapping != null)
            {
                mapping.IsActive = false;
                await uow.quantitative_inspection_mapping.UpdateAsync(mapping, userId, ct);
            }
        }
        #endregion Quantitative Inspection Helpers

        #endregion UpdateItemInspectionCardWithBothInspections Helpers

        #region Custom FluentValidations
        public async Task<bool> IsItemInspectionCardUsed(string Id, CancellationToken ct)
        {
            var itemInspectionCard = _uow.item_inspection_card.GetQueryable().Data.Where(x => x.Id == Id).FirstOrDefault();
            var query = _uow.item.GetQueryable();
            var result = await query.Data.Where(x => itemInspectionCard!=null && x.Id == itemInspectionCard.ItemId)
                .Include(x => x.Production_QC)
                .Include(x => x.Purchase_QC)
                .Include(x => x.Production_QA)
                .FirstOrDefaultAsync();
            return (result?.Purchase_QC?.Any() ?? false) || (result?.Production_QC?.Any() ?? false) || (result?.Production_QA?.Any() ?? false);
        }

        public async Task<bool> IsItemExists(string? ItemCode, CancellationToken ct)
        {
            var query = _uow.item.GetQueryable();
            var result = await query.Data.Where(x => x.ItemCode == ItemCode).FirstOrDefaultAsync();
            return result!=null;
        }
        #endregion Custom FluentValidations
    }
}
