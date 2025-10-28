using BS.Services.ProductionQCService.DTOs;

namespace BS.Services.ProductionQCService
{
    public interface IProductionQCService
    {
        Task<bool> AddProductionQC(AddProductionQCDTO request, string userId, CancellationToken ct);
        Task<List<ResponseProductionQCWithItem>> ListAllProductionQCsWithItem(CancellationToken ct, int lastCount, int skipRecords);
        Task<ResponseProductionQCWithItem> GetProductionQCWithItemById(string Id, CancellationToken ct);
        Task<ResponseProductionQCWithItem> GetProductionQCId(string itemCode, string? docNumber, string? stageType, CancellationToken ct);
        Task<bool> UpdateProductionQC(UpdateProductionQCDTO request, string userId, CancellationToken ct);
        Task<string?> GetProductionQcBMRByQcId(string qcId, CancellationToken ct);
        Task<ListProductionQCResponse> ListAllProductionQCs(int pageSize, int pageNumber, CancellationToken ct);
        Task<int> GetTotalRecords(CancellationToken ct);

        // Custom FluentValidations
        Task<bool> IsItemIdAvailable(string? Id, CancellationToken ct);
    }
}
