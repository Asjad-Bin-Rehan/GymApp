using BS.Services.PurchaseQCSampleService.DTOs;

namespace BS.Services.PurchaseQCSampleService
{
    public interface IPurchaseQCSampleService
    {
        Task<bool> AddPurchaseQCSample(AddPurchaseQCSampleDTO request, string userId, CancellationToken ct);
        Task<List<ResponsePurchaseQCSampleDTO>> ListAllPurchaseQCSamplesByQcId(string qcId, CancellationToken ct, int lastCount, int skipRecords);
        Task<List<ResponsePurchaseQCSampleDTO>> GetPurchaseQCSampleById(CancellationToken ct, string pruchaseQCSampleId);
        Task<bool> UpdatePurchaseQCSample(UpdatePurchaseQCSampleDTO request, string userId, CancellationToken ct);

        // Custom FluentValidations
        Task<bool> IsPurchaseQcIdAvailable(string? Id, CancellationToken ct);
    }
}
