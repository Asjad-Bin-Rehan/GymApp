using BS.Services.PricingService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.PricingService
{
    public interface IPricingService
    {
        Task<UpsertPricingResponse> UpsertPricing(UpsertPricingRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetPricingResponse>> GetPricing(string? pricingId, string? courtId, string? day, int lastCount, int skipRecords, CancellationToken ct);
    }
}
