using BS.Services.UnitOfMeasure.DTOs;
using DA;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.UnitOfMeasure
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private IUnitOfWork _uow;
        public UnitOfMeasureService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<bool> AddUnitOfMeasure(AddUnitOfMeasureDTO request, string userId, CancellationToken ct)
        {
            var entity = request.ToDomain(userId);
            await _uow.unit_of_measure.AddAsync(entity, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<List<ResponseUnitOfMeasure>> ListAllUnitOfMeasures(CancellationToken ct, int lastCount, int skipRecords)
        {
            var query = _uow.unit_of_measure.GetQueryable();
            var result = await query.Data
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Skip(skipRecords)
                                    .Take(lastCount)
                                    .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            var data = result.ToResponseList().ToList();
            return data;
        }

        public async Task<List<ResponseUnitOfMeasure>> GetUnitOfMeasureById(string measureId,CancellationToken ct)
        {
            var query = _uow.unit_of_measure.GetQueryable();
            var result = await query.Data
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Where(x => x.Id == measureId)
                                    .ToListAsync(ct);
            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            var data = result.ToResponseList().ToList();
            return data;
        }


        public async Task<bool> UpdateUnitOfMeasure(UpdateUnitOfMeasureDTO request, string userId, CancellationToken ct)
        {
            var query = _uow.unit_of_measure.GetQueryable();

            var alreadyExists = await query.Data.AnyAsync(x => x.Id != request.Id && (x.Description!=null && request.Description!=null && x.Description.ToLower() == request.Description.ToLower() || x.UoMcode!=null && request.UoMCode!=null && x.UoMcode.ToLower() == request.UoMCode.ToLower()));
            ArgumentFalseException.ThrowIfFalse(!alreadyExists, "Details already exist for another record");

            var result = await query.Data.Where(x => x.Id == request.Id).SingleOrDefaultAsync(ct) ?? throw new ArgumentFalseException("No active record found to update");
            result.UoMcode = request.UoMCode;
            result.Description = request.Description;
            result.IsActive = request.IsActive;
            await _uow.unit_of_measure.UpdateAsync(result, userId, ct);
            await _uow.CommitAsync();
            return true;
        }

        #region Custom FluentValidations
        public async Task<bool> IsUoMUsed(string Id, CancellationToken ct)
        {
            var query = _uow.unit_of_measure.GetQueryable();
            var result = await query.Data.Where(x => x.Id==Id).Include(x => x.Quantitative_Inspection_Mappings).FirstOrDefaultAsync();
            return result?.Quantitative_Inspection_Mappings.Any() ?? false;
        }

        public async Task<bool> IsUoMNameOrCodeExists(string? nameOrCode, CancellationToken ct)
        {
            var query = _uow.unit_of_measure.GetQueryable();
            return await query.Data.AnyAsync(x => x.Description!=null && nameOrCode!=null && x.Description.ToLower() == nameOrCode.ToLower() || x.UoMcode != null && nameOrCode != null && x.UoMcode.ToLower() == nameOrCode.ToLower());
        }
        #endregion Custom FluentValidations
    }
}
