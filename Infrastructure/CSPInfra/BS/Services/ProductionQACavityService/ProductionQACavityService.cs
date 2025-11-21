using BS.Services.ProductionQACavityService.DTOs;
using DA;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.ProductionQACavityService
{
    public class ProductionQACavityService : IProductionQACavityService
    {
        private readonly IUnitOfWork _uow;
        public ProductionQACavityService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> AddProductionQACavity(AddProductionQACavityDTO request, string userId, CancellationToken ct)
        {
            for (int i=1; i<=request.CavityNum; i++)
            {
                var entity = request.ToDomain(userId, i);
                await _uow.production_qa_cavity.AddAsync(entity, userId, ct);
            }

            await _uow.CommitAsync();
            return true;
        }

        public async Task<ResponseProductionQACavity> GetProductionQACavityById(string id, CancellationToken ct)
        {
            var query = _uow.production_qa_cavity.GetQueryable();
            var result = await query.Data.FirstOrDefaultAsync(x => x.Id == id);
            ArgumentFalseException.ThrowIfFalse(result != null, "No record found");
            return result.ToResponse();
        }

        public async Task<List<ResponseProductionQACavity>> ListAllProductionQACavityByQaId(CancellationToken ct, string qaId, int lastCount, int skipRecords)
        {
            var query = _uow.production_qa_cavity.GetQueryable();
            var result = await query.Data
                .Where(x => x.QaId == qaId)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(skipRecords)
                .Take(lastCount)
                .ToListAsync(ct);

            ArgumentFalseException.ThrowIfFalse(result.Any(), "No records found");
            return result.ToResponseList();
        }

        public async Task<bool> UpdateProductionQACavity(UpdateProductionQACavityDTO request, string userId, CancellationToken ct)
        {
            var query = _uow.production_qa_cavity.GetQueryable();
            var entity = await query.Data.FirstOrDefaultAsync(x => x.Id == request.Id, ct) ?? throw new ArgumentFalseException("No active record found to update");
            
            var logEntity = entity.ToDomain(userId);
            await _uow.log_production_qa_cavity.AddAsync(logEntity, userId, ct);

            entity.Name = request.Name;
            entity.InspectionBy = request.InspectionBy;
            entity.InspectionDateTime = request.InspectionDateTime;
            entity.MouldNo = request.MouldNo;
            entity.IsToggledOn = request.IsToggledOn;
            entity.IsCavityPassed = request.IsCavityPassed;
            entity.IsActive = request.IsActive;

            await _uow.production_qa_cavity.UpdateAsync(entity, userId, ct);
            await _uow.CommitAsync();
            return true;
        }
    }
}