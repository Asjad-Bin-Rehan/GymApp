using BS.Services.ItemCardService.DTOs;

namespace BS.Services.ItemCardService
{
    public interface IItemCardService
    {
        Task<bool> AddItem(AddItemCardDTO request, string userId, CancellationToken ct);
        Task<List<ResponseItemCard>> ListAllItems(CancellationToken ct, int lastCount, int skipRecords);
        Task<List<ResponseItemCard>> GetItemById(CancellationToken ct, string itemId);
        Task<List<ResponseItemCard>> GetItemByCode(CancellationToken ct, string itemCode, string? docNum, string? lineNum);
        Task<ResponseItemTypes> GetItemTypesById(string itemId, CancellationToken ct, int lastCount, int skipRecords);
    }
}