using BS.Services.SlotService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.SlotService
{
    public interface ISlotService
    {
        Task<UpsertSlotResponse> UpsertSlot(UpsertSlotRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetSlotResponse>> GetSlot(string? slotId, string? courtId, string? day, bool? isAvailable, int lastCount, int skipRecords, CancellationToken ct);
    }
}
