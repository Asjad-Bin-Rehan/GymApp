using BS.Services.ProductionQACavityService.DTOs;

namespace BS.Services.ProductionQACavityService
{
    public interface IProductionQACavityService
    {
        Task<bool> AddProductionQACavity(AddProductionQACavityDTO request, string userId, CancellationToken ct);
        Task<ResponseProductionQACavity> GetProductionQACavityById(string id, CancellationToken ct);
        Task<List<ResponseProductionQACavity>> ListAllProductionQACavityByQaId(CancellationToken ct, string qaId, int lastCount, int skipRecords);
        Task<bool> UpdateProductionQACavity(UpdateProductionQACavityDTO request, string userId, CancellationToken ct);
    }
}