using System.Reflection.PortableExecutable;
using BS.EnumsAndConstants.Constant;
using BS.Services.InspectionCharactersticService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.InspectionCharactersticService
{
    public class InspectionCharacteristicService : IInspectionCharacteristicService
    {
        IUnitOfWork _uow;
        public InspectionCharacteristicService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<List<ResponseInspectionCharacteristic>> ListAllInspectionCharacteristics(CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.inspection_characteristic.GetQueryable();
            var result = await query.Data
                                    .Include(x => x.Inspection_Attribute)
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Skip(skipRecords)
                                    .Take(lastCount)
                                    .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            var data = result.ToResponseList().ToList();
            return data;
        }

        public async Task<bool> AddCharacteristicWithCriteria(AddCharacteristicWithCriteriaDTO request, string userId, CancellationToken ct)
        {
            if (request.Type!=null && request.Type.ToLower()==KConstantInspectionTypeBS.both)
            {
                var qualitativeCharacteristic = request.ToDomainQualitative(userId);
                await _uow.inspection_characteristic.AddAsync(qualitativeCharacteristic, userId, ct);
                var quantitativeCharacteristic = request.ToDomainQuantitative(userId);
                await _uow.inspection_characteristic.AddAsync(quantitativeCharacteristic, userId, ct);
            }
            else
            {
                var characterstic = request.ToDomain(userId);
                await _uow.inspection_characteristic.AddAsync(characterstic, userId, ct);
            }
            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponseListCharacteristicsWithCriteria>> ListCharacteristicsWithCriteria(CancellationToken ct)
        {
            var query = _uow.inspection_characteristic.GetQueryable();
            var result = await query.Data.Include(x => x.Inspection_Attribute).OrderByDescending(x => x.CreatedDate).ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            return result.ToResponseListCharacteristics();
        }

        public async Task<bool> UpdateCharacterstic(UpdateCharacteristicDTO request, string userId, CancellationToken ct)
        {
            var query = _uow.inspection_characteristic.GetQueryable();

            var alreadyExists = await query.Data.AnyAsync(x => x.Id!=request.Id && x.Description!=null && request.Description!=null && x.AttributeId!=null && request.AttributeId!=null && x.Type!=null && request.Type!=null
                && x.Description.ToLower()==request.Description.ToLower() && x.AttributeId==request.AttributeId && x.Type==request.Type);
            ArgumentFalseException.ThrowIfFalse(!alreadyExists, "Details already exist for another record");

            var result = await query.Data.Where(x => x.Id == request.Id).SingleOrDefaultAsync(ct) ?? throw new ArgumentFalseException("No active record found to update");
            result.AttributeId = request.AttributeId;
            result.Description = request.Description;
            result.Type = request.Type;
            result.SingleCriteria = request.SingleCriteria;
            result.QuantitativeCriteria = request.QuantitativeCriteria;
            result.IsActive = request.IsActive;
            await _uow.inspection_characteristic.UpdateAsync(result, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponseInspectionCharacteristic>> GetInspectionCharacteristicById(CancellationToken ct, string id)
        {
            var query = _uow.inspection_characteristic.GetQueryable();
            var result = await query.Data
                                    .Include(x => x.Inspection_Attribute)
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Where(x => x.Id == id)
                                    .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            var data = result.ToResponseList().ToList();
            return data;
        }

        #region Custom FluentValidations
        public async Task<bool> IsAttributeIdAvailable(string? Id, CancellationToken ct)
        {
            return await _uow.inspection_attribute.GetQueryable().Data.Where(x => x.Id == Id).AnyAsync();
        }

        public async Task<bool> IsCharacteristicUsed(string? Id, CancellationToken ct)
        {
            var query = _uow.inspection_characteristic.GetQueryable();
            var result = await query.Data.Where(x => x.Id == Id)
                .Include(x => x.Inspection_Characteristic_Mappings)
                .Include(x => x.Qualitative_Inspection_Mappings)
                .Include(x => x.Quantitative_Inspection_Mappings)
                .FirstOrDefaultAsync();
            return (result?.Inspection_Characteristic_Mappings?.Any() ?? false) ||
                   (result?.Qualitative_Inspection_Mappings?.Any() ?? false) ||
                   (result?.Quantitative_Inspection_Mappings?.Any() ?? false);
        }

        public async Task<bool> IsCharacteristicDescriptionAndAttributeNotExists(AddCharacteristicWithCriteriaDTO req, CancellationToken ct)
        {
            return await _uow.inspection_characteristic.GetQueryable().Data.Where(x => x.Description!=null && req.Description!=null && x.AttributeId!=null && req.AttributeId!=null && x.Type!=null && req.Type!=null
                && x.Description.ToLower()==req.Description.ToLower() && x.AttributeId==req.AttributeId && x.Type.ToLower()==req.Type.ToLower()
                ).AnyAsync();
        }

        public async Task<bool> IsCharacteristicExists(string? Id, CancellationToken ct)
        {
            return await _uow.inspection_characteristic.GetQueryable().Data.Where(x => x.Id==Id).AnyAsync();
        }
        #endregion Custom FluentValidations
    }
}
