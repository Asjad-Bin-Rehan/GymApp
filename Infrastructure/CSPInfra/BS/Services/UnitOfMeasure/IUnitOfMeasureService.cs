using BS.Services.UnitOfMeasure.DTOs;

namespace BS.Services.UnitOfMeasure
{
    public interface IUnitOfMeasureService
    {
        Task<bool> AddUnitOfMeasure(AddUnitOfMeasureDTO request, string userId, CancellationToken ct);
        Task<List<ResponseUnitOfMeasure>> ListAllUnitOfMeasures(CancellationToken ct, int lastCount, int skipRecords);
        Task<bool> UpdateUnitOfMeasure(UpdateUnitOfMeasureDTO request, string userId, CancellationToken ct);
        Task<List<ResponseUnitOfMeasure>> GetUnitOfMeasureById(string measureId ,CancellationToken ct);

        // Custom FluentValidations
        Task<bool> IsUoMUsed(string uoMId, CancellationToken ct);
        Task<bool> IsUoMNameOrCodeExists(string? nameOrCode, CancellationToken ct);
    }
}
