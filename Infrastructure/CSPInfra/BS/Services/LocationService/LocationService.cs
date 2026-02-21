using System.Linq.Expressions;
using BS.Services.LocationService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.LocationService;

public class LocationService(IUnitOfWork uow) : ILocationService
{
    IUnitOfWork _uow { get; set; } = uow;

    public async Task<UpsertLocationResponse> UpsertLocation(UpsertLocationRequest req, string userId, CancellationToken ct)
    {
        var updateIds = req.Locations.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
        var existingLocations = await _uow.location.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

        List<string> ids = [];
        foreach (var obj in req.Locations)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var location = obj.ToInsert(userId);
                await _uow.location.AddAsync(location, userId, ct);
                ids.Add(location.Id);
            }
            else if (existingLocations.TryGetValue(obj.Id, out var existingLocation))
            {
                var location = existingLocation.ToUpdate(obj);
                await _uow.location.UpdateAsync(location, userId, ct);
                ids.Add(location.Id);
            }
        }

        await _uow.CommitAsync();
        return new UpsertLocationResponse { Ids = ids };
    }

    public async Task<PagedResponse<GetLocationResponse>> GetLocation(string? locationId, string? parentId, string? name, int lastCount, int skipRecords, CancellationToken ct)
    {
        var queryable = _uow.location.GetQueryable();
        Expression<Func<Location, bool>> filter = x => (locationId==null || x.Id==locationId) && (parentId==null || x.ParentId==parentId) || (name==null || x.Name!=null && x.Name.Contains(name)) && x.IsActive;

        var totalCount = await _uow.location.GetQueryable().Data.AsNoTracking().CountAsync(filter);

        var locations = await queryable.Data.AsNoTracking()
                                   .Where(filter)
                                   .OrderByDescending(x => x.CreatedDate)
                                   .Skip(skipRecords)
                                   .Take(lastCount)
                                   .ToListAsync(ct);

        ArgumentFalseException.ThrowIfFalse(locations.Any(), "No locations found.");

        var data = locations.Select(x => x.ToResponse()).ToList();
        return new PagedResponse<GetLocationResponse>() { TotalCount = totalCount, Data = data };
    }
}