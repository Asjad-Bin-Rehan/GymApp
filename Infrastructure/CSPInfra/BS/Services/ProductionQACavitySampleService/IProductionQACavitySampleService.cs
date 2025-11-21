using BS.Services.ProductionQACavitySampleService.DTOs;

namespace BS.Services.ProductionQACavitySampleService
{
    public interface IProductionQACavitySampleService
    {
        Task<bool> AddProductionQACavitySample(AddProductionQACavitySampleDTO request, string userId, CancellationToken ct);

        Task<List<ResponseProductionQACavitySample>> ListAllProductionQACavitySamplesByCavityId(string cavityId, CancellationToken ct, int lastCount, int skipRecords);

        Task<List<ResponseProductionQACavitySample>> GetProductionQACavitySampleById(CancellationToken ct, string sampleId);

        Task<bool> UpdateProductionQACavitySample(UpdateProductionQACavitySampleDTO request, string userId, CancellationToken ct);

        // Custom FluentValidations
        Task<bool> IsCavityIdAvailable(string? Id, CancellationToken ct);
    }
}
