using BS.Services.PurchaseQCService.DTOs;

namespace BS.Services.PurchaseQCService
{
    public interface IPurchaseQCService
    {
        Task<bool> AddPurchaseQC(AddPurchaseQCDTO request, string userId, CancellationToken ct);
        Task<List<ResponsePurchaseQCWithItem>> ListAllPurchaseQCsWithItem(CancellationToken ct, int lastCount, int skipRecords);
        Task<ResponsePurchaseQCWithItem> GetPurchaseQCWithItemById(string Id, CancellationToken ct);
        Task<double> GetSampleQuantity(string itemId, int inspectionQuantity, CancellationToken ct);
        Task<ResponsePurchaseQCWithItem> GetPurchaseQCId(string itemCode, string? docNumber, int? lineNo, string? stageType, CancellationToken ct);
        Task<bool> UpdatePurchaseQC(UpdatePurchaseQCDTO request, string userId, CancellationToken ct);
        Task<ListPurchaseQCResponse> ListAllPurchaseQCs(int pageSize, int pageNumber, CancellationToken ct);
        Task<int> GetTotalRecords(CancellationToken ct);

        // Custom FluentValidations
        Task<bool> IsItemIdAvailable(string? Id, CancellationToken ct);
    }
}
