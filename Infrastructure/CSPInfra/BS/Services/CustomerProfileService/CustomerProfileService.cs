using System.Linq.Expressions;
using BS.Services.CustomerProfileService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.CustomerProfileService;

public class CustomerProfileService(IUnitOfWork uow) : ICustomerProfileService
{
    IUnitOfWork _uow { get; set; } = uow;

    public async Task<UpsertCustomerProfileResponse> UpsertCustomerProfile(UpsertCustomerProfileRequest req, string userId, CancellationToken ct)
    {
        var updateIds = req.CustomerProfiles.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
        var existingProfiles = await _uow.customer_profile.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

        List<string> ids = [];
        foreach (var obj in req.CustomerProfiles)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var profile = obj.ToInsert(userId);
                await _uow.customer_profile.AddAsync(profile, userId, ct);
                ids.Add(profile.Id);
            }
            else if (existingProfiles.TryGetValue(obj.Id, out var existingProfile))
            {
                var profile = existingProfile.ToUpdate(obj);
                await _uow.customer_profile.UpdateAsync(profile, userId, ct);
                ids.Add(profile.Id);
            }
        }

        await _uow.CommitAsync();
        return new UpsertCustomerProfileResponse { Ids = ids };
    }

    public async Task<PagedResponse<GetCustomerProfileResponse>> GetCustomerProfile(string? customerProfileId, string? userId, int lastCount, int skipRecords, CancellationToken ct)
    {
        var queryable = _uow.customer_profile.GetQueryable();
        Expression<Func<CustomerProfile, bool>> filter = x =>
            (customerProfileId == null || x.Id == customerProfileId)
            && (userId == null || x.UserId == userId)
            && x.IsActive;

        var totalCount = await queryable.Data.AsNoTracking().CountAsync(filter, ct);

        var profiles = await queryable.Data.AsNoTracking()
            .Where(filter)
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);

        ArgumentFalseException.ThrowIfFalse(profiles.Any(), "No customer profiles found.");

        var data = profiles.Select(x => x.ToResponse()).ToList();
        return new PagedResponse<GetCustomerProfileResponse> { TotalCount = totalCount, Data = data };
    }
}
