using BS.Services.ItemInspectionCardService.DTOs;

namespace BS.Services.ItemInspectionCardService
{
    public interface IItemInspectionCardService
    {
        Task<List<ResponseCardWithBothCharacteristicsDTO>> ListAllCardsWithBothCharacteristics(CancellationToken ct, int lastCount, int skipRecords);
        Task<ResponseCardWithBothCharacteristicsDTO> GetCardByIdWithBothCharacteristics(string itemInspectionCardId, CancellationToken ct);
        Task<ResponseCardWithBothCharacteristicsDTO> GetCardByCodeWithBothCharacteristics(string itemCode, CancellationToken ct);
        Task<bool> AddItemInspectionCardWithBothInspections(AddItemInspectionCardWithBothInspectionsDTO request, string userId, CancellationToken ct);
        Task<bool> UpdateItemInspectionCard(UpdateItemInspectionCardDTO request, string userId, CancellationToken ct);
        Task<List<ResponseCardWithBothCharacteristicsDTO>> ListCardByCodeWithBothCharacteristics(string itemCode, CancellationToken ct);

        // Custom FluentValidations
        Task<bool> IsItemInspectionCardUsed(string Id, CancellationToken ct);
        Task<bool> IsItemExists(string? ItemCode, CancellationToken ct);
    }
}
