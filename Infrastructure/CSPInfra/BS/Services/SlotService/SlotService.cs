using System.Linq.Expressions;
using BS.Services.SlotService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.SlotService;

public class SlotService(IUnitOfWork uow) : ISlotService
{
    IUnitOfWork _uow { get; set; } = uow;

    public async Task<UpsertSlotResponse> UpsertSlot(UpsertSlotRequest req, string userId, CancellationToken ct)
    {
        var updateIds = req.Slots.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
        var existingSlots = await _uow.slot.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

        List<string> ids = [];
        foreach (var obj in req.Slots)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var slot = obj.ToInsert(userId);
                await _uow.slot.AddAsync(slot, userId, ct);
                ids.Add(slot.Id);
            }
            else if (existingSlots.TryGetValue(obj.Id, out var existingSlot))
            {
                var slot = existingSlot.ToUpdate(obj);
                await _uow.slot.UpdateAsync(slot, userId, ct);
                ids.Add(slot.Id);
            }
        }

        await _uow.CommitAsync();
        return new UpsertSlotResponse { Ids = ids };
    }

    public async Task<PagedResponse<GetSlotResponse>> GetSlot(string? slotId, string? courtId, string? day, bool? isAvailable, int lastCount, int skipRecords, CancellationToken ct)
    {
        var queryable = _uow.slot.GetQueryable();
        Expression<Func<Slot, bool>> filter = x =>
            (slotId == null || x.Id == slotId)
            && (courtId == null || x.CourtId == courtId)
            && (day == null || x.Day == day)
            && (isAvailable == null || x.IsAvailable == isAvailable)
            && x.IsActive;

        var totalCount = await queryable.Data.AsNoTracking().CountAsync(filter, ct);

        var slots = await queryable.Data.AsNoTracking()
            .Where(filter)
            .OrderByDescending(x => x.StartDateTime)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);

        ArgumentFalseException.ThrowIfFalse(slots.Any(), "No slots found.");

        var data = slots.Select(x => x.ToResponse()).ToList();
        return new PagedResponse<GetSlotResponse> { TotalCount = totalCount, Data = data };
    }
}
