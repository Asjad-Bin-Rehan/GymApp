using BS.Services.InspectionAttributeService.DTOs;

namespace BS.Services.InspectionAttributeService
{
    public interface IInspectionAttributeService
    {
        Task<bool> AddInspectionAttribute(AddInspectionAttributeDTO request, string userId, CancellationToken ct);
        Task<List<ResponseInspectionAttribute>> ListAllInspectionAttributesRaw(CancellationToken ct);
        Task<List<ResponseInspectionAttribute>> ListAllInspectionAttributes(int lastCount, int skipRecords, CancellationToken ct);
        Task<ResponseInspectionAttribute> GetInspectionAttributeById(string id, CancellationToken ct);
        Task<bool> UpdateInspectionAttribute(UpdateInspectionAttributeDTO request, string userId, CancellationToken ct);

        // Custom FluentValidations
        Task<bool> IsInspectionAttributeNameExists(string? description, CancellationToken ct);
    }
}
