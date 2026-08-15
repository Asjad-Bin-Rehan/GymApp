using BS.Services.VenueService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.VenueService
{
    public interface IVenueService
    {
        Task<UpsertVenueResponse> UpsertVenue(UpsertVenueRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetVenueResponse>> GetVenue(string? venueId, string? organizationId, string? locationId, string? name, int lastCount, int skipRecords, CancellationToken ct);
    }
}
