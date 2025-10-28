using BS.Services.ProductionQCSampleService.DTOs;

namespace BS.Services.ProductionQCSampleService
{
    public interface IProductionQCSampleService
    {
        Task<bool> AddProductionQCSample(AddProductionQCSampleDTO request, string userId, CancellationToken ct);
        Task<List<ResponseProductionQCSample>> ListAllProductionQCSamplesByQcId(string qcId, CancellationToken ct, int lastCount, int skipRecords);
        Task<List<ResponseProductionQCSample>> GetProductionQCSampleById(CancellationToken ct, string productionQCSampleId);
        Task<bool> UpdateProductionQCSample(UpdateProductionQCSampleDTO request, string userId, CancellationToken ct);

        // Custom FluentValidations
        Task<bool> IsProductionQcIdAvailable(string? Id, CancellationToken ct);
    }
}
