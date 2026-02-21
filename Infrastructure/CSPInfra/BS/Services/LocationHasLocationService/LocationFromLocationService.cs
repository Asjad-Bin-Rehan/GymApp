using System.Linq.Expressions;
using BS.Services.LocationHasLocationService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.LocationHasLocationService
{
    public class LocationFromLocationService(IUnitOfWork uow) : ILocationFromLocationService
    {
        private readonly IUnitOfWork _uow = uow;

        public async Task<UpsertLocationFromLocationResponse> UpsertLocationFromLocation(UpsertLocationFromLocationRequest req, string userId, CancellationToken ct)
        {
            var updateIds = req.Location_From_Locations.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
            var existingMatrix = await _uow.location_from_location.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

            List<string> ids = [];
            foreach (var obj in req.Location_From_Locations)
            {
                if (string.IsNullOrEmpty(obj.Id))
                {
                    var entry = obj.ToInsert(userId);
                    await _uow.location_from_location.AddAsync(entry, userId, ct);
                    ids.Add(entry.Id);
                }
                else if (existingMatrix.TryGetValue(obj.Id, out var existing))
                {
                    existing.ToUpdate(obj);
                    await _uow.location_from_location.UpdateAsync(existing, userId, ct);
                    ids.Add(existing.Id);
                }
            }

            await _uow.CommitAsync();
            return new UpsertLocationFromLocationResponse { Ids = ids };
        }

        public async Task<PagedResponse<GetLocationFromLocationResponse>> GetLocationFromLocation(string? locationId, int? isNearby, int lastCount, int skipRecords, CancellationToken ct)
        {
            var queryable = _uow.location_from_location.GetQueryable();
            Expression<Func<Location_From_Location, bool>> filter = x => (locationId == null || x.LocationId == locationId) && (isNearby == null || x.NearbyRank == isNearby) && x.IsActive;

            var totalCount = await queryable.Data.AsNoTracking().CountAsync(filter, ct);

            var results = await queryable.Data.AsNoTracking()
                .Include(x => x.OtherLocation)
                .Where(filter)
                .OrderBy(x => x.Distance)
                .Skip(skipRecords)
                .Take(lastCount)
                .ToListAsync(ct);

            ArgumentFalseException.ThrowIfFalse(results.Any(), "No nearby grounds found.");

            var data = results.Select(x => x.ToResponse()).ToList();
            return new PagedResponse<GetLocationFromLocationResponse> { TotalCount = totalCount, Data = data };
        }
    }
}