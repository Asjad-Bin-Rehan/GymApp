using BS.Services.CourtService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.CourtService
{
    public interface ICourtService
    {
        Task<UpsertCourtResponse> UpsertCourt(UpsertCourtRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetCourtResponse>> GetCourt(string? courtId, string? venueId, string? name, string? code, string? surfaceType, int lastCount, int skipRecords, CancellationToken ct);
    }
}
