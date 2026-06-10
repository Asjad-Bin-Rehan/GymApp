using System.Linq.Expressions;
using BS.Services.TenantService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.TenantService;

public class TenantService(IUnitOfWork uow) : ITenantService
{
    IUnitOfWork _uow { get; set; } = uow;

    public async Task<UpsertTenantResponse> UpsertTenant(UpsertTenantRequest req, string userId, CancellationToken ct)
    {
        var updateIds = req.Tenants.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
        var existingTenants = await _uow.tenant.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

        List<string> ids = [];
        foreach (var obj in req.Tenants)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var tenant = obj.ToInsert(userId);
                await _uow.tenant.AddAsync(tenant, userId, ct);
                ids.Add(tenant.Id);
            }
            else if (existingTenants.TryGetValue(obj.Id, out var existingTenant))
            {
                var tenant = existingTenant.ToUpdate(obj);
                await _uow.tenant.UpdateAsync(tenant, userId, ct);
                ids.Add(tenant.Id);
            }
        }

        await _uow.CommitAsync();
        return new UpsertTenantResponse { Ids = ids };
    }

    public async Task<PagedResponse<GetTenantResponse>> GetTenant(string? tenantId, string? name, int lastCount, int skipRecords, CancellationToken ct)
    {
        var queryable = _uow.tenant.GetQueryable();
        Expression<Func<Tenant, bool>> filter = x =>
            (tenantId == null || x.Id == tenantId)
            && (name == null || x.Name != null && x.Name.Contains(name))
            && x.IsActive;

        var totalCount = await queryable.Data.AsNoTracking().CountAsync(filter, ct);

        var tenants = await queryable.Data.AsNoTracking()
            .Where(filter)
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);

        ArgumentFalseException.ThrowIfFalse(tenants.Any(), "No tenants found.");

        var data = tenants.Select(x => x.ToResponse()).ToList();
        return new PagedResponse<GetTenantResponse> { TotalCount = totalCount, Data = data };
    }
}
