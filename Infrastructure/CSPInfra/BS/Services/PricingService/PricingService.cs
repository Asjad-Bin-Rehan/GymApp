using System.Linq.Expressions;
using BS.Services.PricingService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.PricingService;

public class PricingService(IUnitOfWork uow) : IPricingService
{
    IUnitOfWork _uow { get; set; } = uow;

    public async Task<UpsertPricingResponse> UpsertPricing(UpsertPricingRequest req, string userId, CancellationToken ct)
    {
        var updateIds = req.Pricings.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
        var existingPricings = await _uow.pricing.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

        List<string> ids = [];
        foreach (var obj in req.Pricings)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var pricing = obj.ToInsert(userId);
                await _uow.pricing.AddAsync(pricing, userId, ct);
                ids.Add(pricing.Id);
            }
            else if (existingPricings.TryGetValue(obj.Id, out var existingPricing))
            {
                var pricing = existingPricing.ToUpdate(obj);
                await _uow.pricing.UpdateAsync(pricing, userId, ct);
                ids.Add(pricing.Id);
            }
        }

        await _uow.CommitAsync();
        return new UpsertPricingResponse { Ids = ids };
    }

    public async Task<PagedResponse<GetPricingResponse>> GetPricing(string? pricingId, string? courtId, string? day, int lastCount, int skipRecords, CancellationToken ct)
    {
        var queryable = _uow.pricing.GetQueryable();
        Expression<Func<Pricing, bool>> filter = x =>
            (pricingId == null || x.Id == pricingId)
            && (courtId == null || x.CourtId == courtId)
            && (day == null || x.Day == day)
            && x.IsActive;

        var totalCount = await queryable.Data.AsNoTracking().CountAsync(filter, ct);

        var pricings = await queryable.Data.AsNoTracking()
            .Where(filter)
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);

        ArgumentFalseException.ThrowIfFalse(pricings.Any(), "No pricings found.");

        var data = pricings.Select(x => x.ToResponse()).ToList();
        return new PagedResponse<GetPricingResponse> { TotalCount = totalCount, Data = data };
    }
}
