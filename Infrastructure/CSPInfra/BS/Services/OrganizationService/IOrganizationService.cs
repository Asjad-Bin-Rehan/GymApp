using BS.Services.OrganizationService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.OrganizationService
{
    public interface IOrganizationService
    {
        Task<UpsertOrganizationResponse> UpsertOrganization(UpsertOrganizationRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetOrganizationResponse>> GetOrganization(string? organizationId, string? tenantId, string? name, int lastCount, int skipRecords, CancellationToken ct);
    }
}
