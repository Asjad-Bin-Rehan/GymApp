using BS.Services.InspectionCharactersticService.DTOs;

namespace BS.Services.InspectionCharactersticService
{
    public interface IInspectionCharacteristicService
    {
        Task<bool> AddCharacteristicWithCriteria(AddCharacteristicWithCriteriaDTO request, string userId, CancellationToken ct);
        Task<bool> UpdateCharacterstic(UpdateCharacteristicDTO request, string userId, CancellationToken ct);
        Task<List<ResponseListCharacteristicsWithCriteria>> ListCharacteristicsWithCriteria(CancellationToken ct);
        Task<List<ResponseInspectionCharacteristic>> ListAllInspectionCharacteristics(CancellationToken cancellationToken, int lastCount, int skipRecords);
        Task<List<ResponseInspectionCharacteristic>> GetInspectionCharacteristicById(CancellationToken ct, string id);

        // Custom FluentValidations
        Task<bool> IsAttributeIdAvailable(string? Id, CancellationToken ct);
        Task<bool> IsCharacteristicUsed(string? Id, CancellationToken ct);
        Task<bool> IsCharacteristicDescriptionAndAttributeNotExists(AddCharacteristicWithCriteriaDTO req, CancellationToken ct);
        Task<bool> IsCharacteristicExists(string? Id, CancellationToken ct);
    }
}
