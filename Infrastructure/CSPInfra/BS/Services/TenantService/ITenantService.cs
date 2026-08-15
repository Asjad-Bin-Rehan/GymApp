using BS.Services.TenantService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.TenantService
{
    public interface ITenantService
    {
        Task<UpsertTenantResponse> UpsertTenant(UpsertTenantRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetTenantResponse>> GetTenant(string? tenantId, string? name, int lastCount, int skipRecords, CancellationToken ct);
    }
}
