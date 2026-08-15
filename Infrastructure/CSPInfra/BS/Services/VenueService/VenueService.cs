using System.Linq.Expressions;
using BS.Services.VenueService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.VenueService;

public class VenueService(IUnitOfWork uow) : IVenueService
{
    IUnitOfWork _uow { get; set; } = uow;

    public async Task<UpsertVenueResponse> UpsertVenue(UpsertVenueRequest req, string userId, CancellationToken ct)
    {
        var updateIds = req.Venues.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
        var existingVenues = await _uow.venue.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

        List<string> ids = [];
        foreach (var obj in req.Venues)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var venue = obj.ToInsert(userId);
                await _uow.venue.AddAsync(venue, userId, ct);
                ids.Add(venue.Id);
            }
            else if (existingVenues.TryGetValue(obj.Id, out var existingVenue))
            {
                var venue = existingVenue.ToUpdate(obj);
                await _uow.venue.UpdateAsync(venue, userId, ct);
                ids.Add(venue.Id);
            }
        }

        await _uow.CommitAsync();
        return new UpsertVenueResponse { Ids = ids };
    }

    public async Task<PagedResponse<GetVenueResponse>> GetVenue(string? venueId, string? organizationId, string? locationId, string? name, int lastCount, int skipRecords, CancellationToken ct)
    {
        var queryable = _uow.venue.GetQueryable();
        Expression<Func<Venue, bool>> filter = x =>
            (venueId == null || x.Id == venueId)
            && (organizationId == null || x.OrganizationId == organizationId)
            && (locationId == null || x.LocationId == locationId)
            && (name == null || x.Name != null && x.Name.Contains(name))
            && x.IsActive;

        var totalCount = await queryable.Data.AsNoTracking().CountAsync(filter, ct);

        var venues = await queryable.Data.AsNoTracking()
            .Where(filter)
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);

        ArgumentFalseException.ThrowIfFalse(venues.Any(), "No venues found.");

        var data = venues.Select(x => x.ToResponse()).ToList();
        return new PagedResponse<GetVenueResponse> { TotalCount = totalCount, Data = data };
    }
}
