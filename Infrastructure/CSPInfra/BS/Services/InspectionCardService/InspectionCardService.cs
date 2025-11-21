using BS.Services.InspectionCardService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.InspectionCardService
{
    public class InspectionCardService : IInspectionCardService
    {
        private IUnitOfWork _uow;
        public InspectionCardService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<bool> AddInspectionCardWithCharacteristics(AddInspectionCardWithCharacteristicsDTO request, string userId, CancellationToken ct)
        {
            var inspectionCard = request.ToDomain(userId);
            await _uow.inspection_card.AddAsync(inspectionCard, userId, ct);

            var result = await _uow.inspection_characteristic.GetAsync(ct, x => request.CharacteristicsIds.Distinct().Contains(x.Id));
            ArgumentFalseException.ThrowIfFalse(!request.CharacteristicsIds.Distinct().Except(result.Data.Select(x => x.Id)).ToList().Any(), "Characteristics doesnt exist");
            foreach (var charactersticId in request.CharacteristicsIds)
            {
                var charactersticmapping = request.ToDomain(userId, charactersticId, inspectionCard.Id);
                await _uow.inspection_characteristic_mapping.AddAsync(charactersticmapping, userId, ct);
            }

            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponseCardWithCharacteristics>> ListAllCharacteristicsByInspectionCardId(string inspectionCardId, CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.inspection_card.GetQueryable();
            var result = await query.Data
                                    .Include(x => x.Inspection_Characteristic_Mappings
                                    .Where(mapping => mapping.IsActive))
                                    .ThenInclude(x => x.Inspection_Characteristic)
                                    .ThenInclude(x => x.Inspection_Attribute)
                                    .Where(x => x.Id == inspectionCardId && x.Inspection_Characteristic_Mappings.Any(mapping => mapping.IsActive))
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Skip(skipRecords)
                                    .Take(lastCount)
                                    .ToListAsync(ct);

            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");

            var data = result.Select(x => x.ToResponse(true)).ToList();
            return data;
        }

        public async Task<ResponseCardWithCriteriaWithCharacteristics> GetCardByIdWithCharacteristicsWithCriteria(string inspectionCardId, CancellationToken ct)
        {
            var query = _uow.inspection_card.GetQueryable();
            var cardData = await query.Data
                                      .Include(x => x.Inspection_Characteristic_Mappings.Where(mapping => mapping.IsActive))
                                        .ThenInclude(mapping => mapping.Inspection_Characteristic)
                                        .ThenInclude(x => x.Inspection_Attribute)
                                      .Where(x => x.Id == inspectionCardId && x.Inspection_Characteristic_Mappings.Any(x => x.IsActive))
                                      .FirstOrDefaultAsync(ct);
            ArgumentFalseException.ThrowIfFalse(cardData != null, "No record found");

            var response = new ResponseCardWithCriteriaWithCharacteristics
            {
                Id = cardData?.Id ?? string.Empty,
                IntCode = cardData?.IntCode,
                Description = cardData?.Description,
                ResponseCharacteristicWithCriteria = cardData?.Inspection_Characteristic_Mappings
                                                    .Select(mapping => mapping.Inspection_Characteristic.ToResponseWithCriteria())
                                                    .ToList() ?? []
            };
            return response;
        }

        public async Task<bool> UpdateInspectionCard(UpdateInspectionCardDTO request, string userId, CancellationToken ct)
        {
            var query = _uow.inspection_card.GetQueryable();

            var alreadyExists = await query.Data.AnyAsync(x => x.Id != request.Id && x.Description != null && request.Description != null && x.Description.ToLower() == request.Description.ToLower());
            ArgumentFalseException.ThrowIfFalse(!alreadyExists, "Details already exist for another record");

            var result = await query.Data.Include(x => x.Inspection_Characteristic_Mappings).FirstOrDefaultAsync(x => x.Id == request.Id);
            ArgumentFalseException.ThrowIfFalse(result != null, "No records found");

            result!.Description = request.Description;
            result.IsActive = request.IsActive;
            await _uow.inspection_card.UpdateAsync(result, userId, ct);

            #region Detach Mappings
            var mappingsToDetach = result.Inspection_Characteristic_Mappings
                .Where(x => x.CharacteristicId != null && request.DetachInspectionCharacteristicIds.Contains(x.CharacteristicId) && x.InspectionCardId == request.Id && x.IsActive).ToList();

            foreach (var mapping in mappingsToDetach)
            {
                mapping.IsActive = false;
                await _uow.inspection_characteristic_mapping.UpdateAsync(mapping, userId, ct);
            }
            #endregion Detach Mappings

            #region Add New Characteristic Mappings Only
            foreach (var characteristicId in request.CharacteristicsIds.Distinct())
            {
                if (!result.Inspection_Characteristic_Mappings.Any(m => m.CharacteristicId == characteristicId && m.IsActive))
                {
                    var newMapping = request.ToDomain(characteristicId, userId);
                    await _uow.inspection_characteristic_mapping.AddAsync(newMapping, userId, ct);
                }
            }
            #endregion Add New Characteristic Mappings Only

            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponseInspectionCard>> ListAllInspectionCards(CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.inspection_card.GetQueryable();
            var result = await query.Data
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Skip(skipRecords)
                                    .Take(lastCount)
                                    .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            var data = result.ToResponseList().ToList();
            return data;
        }

        public async Task<List<ResponseInspectionCard>> GetInspectionCardById(CancellationToken ct, string inspectionCardId)
        {
            var query = _uow.inspection_card.GetQueryable();
            var result = await query.Data
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Where(x => x.Id == inspectionCardId)
                                    .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            var data = result.ToResponseList().ToList();
            return data;
        }

        #region Custom FluentValidations
        public async Task<bool> IsDescriptionExists(string? description, CancellationToken ct)
        {
            var result = await _uow.inspection_card.AnyAsync(ct, x => x.Description != null && description != null && x.Description.ToLower() == description.ToLower());
            return result.Data;
        }

        public async Task<bool> IsCardIdAvailable(string? Id, CancellationToken ct)
        {
            var result = await _uow.inspection_characteristic.AnyAsync(ct, x => x.Id == Id);
            return result.Data;
        }
        
        public async Task<bool> IsInspectionCardUsed(string? Id, CancellationToken ct)
        {
            var query = _uow.inspection_card.GetQueryable();
            var result = await query.Data.Where(x => x.Id == Id).Include(x => x.Item_Inspection_Cards).FirstOrDefaultAsync();
            return result?.Item_Inspection_Cards?.Any() ?? false;
        }
        #endregion Custom FluentValidations
    }
}
