using BS.Services.LocationHasLocationService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.LocationHasLocationService
{
    public interface ILocationFromLocationService
    {
        Task<UpsertLocationFromLocationResponse> UpsertLocationFromLocation(UpsertLocationFromLocationRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetLocationFromLocationResponse>> GetLocationFromLocation(string? locationId, int? isNearby, int lastCount, int skipRecords, CancellationToken ct);
    }
}