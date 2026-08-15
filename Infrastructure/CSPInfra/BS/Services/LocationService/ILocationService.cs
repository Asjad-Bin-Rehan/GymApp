using BS.Services.LocationService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.LocationService
{
    public interface ILocationService
    {
        Task<UpsertLocationResponse> UpsertLocation(UpsertLocationRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetLocationResponse>> GetLocation(string? locationId, string? parentId, string? name, int lastCount, int skipRecords, CancellationToken ct);
    }
}