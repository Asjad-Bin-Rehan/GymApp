using BS.Services.ItemSampleService.DTO;

namespace BS.Services.ItemSampleService;

public interface IItemSampleService
{
    Task<bool> AddSampleWithRanges(AddItemSampleWithRangesDTO request, string userId, CancellationToken ct);
    Task<List<ResponseItemSampleWithSamplingRanges>> ListRangesBySampleId(string sampleId, CancellationToken ct, int lastCount, int skipRecords);
    Task<List<ResponseItemSample>> ListAllItemSamplesWithItems(CancellationToken ct, int lastCount, int skipRecords);
    Task<List<ResponseItemSample>> GetItemSampleById(string sampleId , CancellationToken ct);
    Task<ResponseItemSample> GetItemSampleByItemId(string ItemId, CancellationToken ct);
    Task<bool> UpdateSampleWithRanges(UpdateItemSampleDTO request, string userId, CancellationToken ct);

    // Custom FluentValidations
    Task<bool> IsItemIdAvailable(string? itemId, CancellationToken ct);
    Task<bool> IsSampleIdAvailable(string? Id, CancellationToken ct);
    Task<bool> IsSampleItemExists(string? itemId, CancellationToken ct);
    Task<bool> IsItemSampleUsed(string? Id, CancellationToken ct);

}