using System.Linq.Expressions;
using BS.Services.CourtService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.CourtService;

public class CourtService(IUnitOfWork uow) : ICourtService
{
    IUnitOfWork _uow { get; set; } = uow;

    public async Task<UpsertCourtResponse> UpsertCourt(UpsertCourtRequest req, string userId, CancellationToken ct)
    {
        var updateIds = req.Courts.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
        var existingCourts = await _uow.court.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

        List<string> ids = [];
        foreach (var obj in req.Courts)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var court = obj.ToInsert(userId);
                await _uow.court.AddAsync(court, userId, ct);
                ids.Add(court.Id);
            }
            else if (existingCourts.TryGetValue(obj.Id, out var existingCourt))
            {
                var court = existingCourt.ToUpdate(obj);
                await _uow.court.UpdateAsync(court, userId, ct);
                ids.Add(court.Id);
            }
        }

        await _uow.CommitAsync();
        return new UpsertCourtResponse { Ids = ids };
    }

    public async Task<PagedResponse<GetCourtResponse>> GetCourt(string? courtId, string? venueId, string? name, string? code, string? surfaceType, int lastCount, int skipRecords, CancellationToken ct)
    {
        var queryable = _uow.court.GetQueryable();
        Expression<Func<Court, bool>> filter = x =>
            (courtId == null || x.Id == courtId)
            && (venueId == null || x.VenueId == venueId)
            && (name == null || x.Name != null && x.Name.Contains(name))
            && (code == null || x.Code == code)
            && (surfaceType == null || x.SurfaceType == surfaceType)
            && x.IsActive;

        var totalCount = await queryable.Data.AsNoTracking().CountAsync(filter, ct);

        var courts = await queryable.Data.AsNoTracking()
            .Where(filter)
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);

        ArgumentFalseException.ThrowIfFalse(courts.Any(), "No courts found.");

        var data = courts.Select(x => x.ToResponse()).ToList();
        return new PagedResponse<GetCourtResponse> { TotalCount = totalCount, Data = data };
    }
}
