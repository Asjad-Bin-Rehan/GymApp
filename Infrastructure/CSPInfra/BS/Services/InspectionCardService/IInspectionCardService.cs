using BS.Services.InspectionCardService.DTOs;

namespace BS.Services.InspectionCardService
{
    public interface IInspectionCardService
    {
        Task<bool> AddInspectionCardWithCharacteristics(AddInspectionCardWithCharacteristicsDTO request, string userId, CancellationToken ct);
        Task<bool> UpdateInspectionCard(UpdateInspectionCardDTO request, string userId, CancellationToken ct);
        Task<List<ResponseInspectionCard>> GetInspectionCardById(CancellationToken ct, string inspectionCardId);
        Task<ResponseCardWithCriteriaWithCharacteristics> GetCardByIdWithCharacteristicsWithCriteria(string inspectionCardId, CancellationToken ct);
        Task<List<ResponseInspectionCard>> ListAllInspectionCards(CancellationToken ct, int lastCount, int skipRecords);
        Task<List<ResponseCardWithCharacteristics>> ListAllCharacteristicsByInspectionCardId(string inspectionCardId, CancellationToken ct, int lastCount, int skipRecords);

        // Custom FluentValidations
        Task<bool> IsDescriptionExists(string? description, CancellationToken ct);
        Task<bool> IsCardIdAvailable(string? Id, CancellationToken ct);
        Task<bool> IsInspectionCardUsed(string? CardId, CancellationToken ct);
    }
}
