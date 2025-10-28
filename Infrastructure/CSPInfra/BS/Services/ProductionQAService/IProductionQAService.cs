using BS.Services.ProductionQAService.DTOs;

namespace BS.Services.ProductionQAService
{
    public interface IProductionQAService
    {
        Task<bool> AddProductionQA(AddProductionQADTO request, string userId, CancellationToken ct);
        Task<List<ResponseProductionQAWithItem>> ListAllProductionQAsWithItem(CancellationToken ct, int lastCount, int skipRecords);
        Task<ResponseProductionQAWithItem> GetProductionQAWithItemById(string id, CancellationToken ct);
        Task<bool> UpdateProductionQA(UpdateProductionQADTO request, string userId, CancellationToken ct);
        Task<ResponseProductionQAWithItem> GetProductionQAId(string itemCode, string? docNumber, string? stageType, CancellationToken ct);
        Task<string?> GetProductionQaBMRByQaId(string qaId, CancellationToken ct);
        Task<List<ResponseProductionQaReportCavityWise>> GetProductionQaReportCavityWiseById(string qaId, CancellationToken ct);
        Task<ListProductionQAResponse> ListAllProductionQAs(int pageSize, int pageNumber, CancellationToken ct);
        Task<int> GetTotalRecords(CancellationToken ct);

        // Custom FluentValidations
        Task<bool> IsItemIdAvailable(string? Id, CancellationToken ct);
    }
}
