using BS.Services.CustomerProfileService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.CustomerProfileService
{
    public interface ICustomerProfileService
    {
        Task<UpsertCustomerProfileResponse> UpsertCustomerProfile(UpsertCustomerProfileRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetCustomerProfileResponse>> GetCustomerProfile(string? customerProfileId, string? userId, int lastCount, int skipRecords, CancellationToken ct);
    }
}
