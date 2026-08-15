using System.Linq.Expressions;
using BS.Services.OrganizationService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.CommonModels;
using Helpers.CustomExceptionThrower;
using Microsoft.EntityFrameworkCore;

namespace BS.Services.OrganizationService;

public class OrganizationService(IUnitOfWork uow) : IOrganizationService
{
    IUnitOfWork _uow { get; set; } = uow;

    public async Task<UpsertOrganizationResponse> UpsertOrganization(UpsertOrganizationRequest req, string userId, CancellationToken ct)
    {
        var updateIds = req.Organizations.Where(x => !string.IsNullOrEmpty(x.Id)).Select(x => x.Id!).ToList();
        var existingOrganizations = await _uow.organization.GetQueryable().Data.Where(x => updateIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);

        List<string> ids = [];
        foreach (var obj in req.Organizations)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var organization = obj.ToInsert(userId);
                await _uow.organization.AddAsync(organization, userId, ct);
                ids.Add(organization.Id);
            }
            else if (existingOrganizations.TryGetValue(obj.Id, out var existingOrganization))
            {
                var organization = existingOrganization.ToUpdate(obj);
                await _uow.organization.UpdateAsync(organization, userId, ct);
                ids.Add(organization.Id);
            }
        }

        await _uow.CommitAsync();
        return new UpsertOrganizationResponse { Ids = ids };
    }

    public async Task<PagedResponse<GetOrganizationResponse>> GetOrganization(string? organizationId, string? tenantId, string? name, int lastCount, int skipRecords, CancellationToken ct)
    {
        var queryable = _uow.organization.GetQueryable();
        Expression<Func<Organization, bool>> filter = x =>
            (organizationId == null || x.Id == organizationId)
            && (tenantId == null || x.TenantId == tenantId)
            && (name == null || x.Name != null && x.Name.Contains(name))
            && x.IsActive;

        var totalCount = await queryable.Data.AsNoTracking().CountAsync(filter, ct);

        var organizations = await queryable.Data.AsNoTracking()
            .Where(filter)
            .OrderByDescending(x => x.CreatedDate)
            .Skip(skipRecords)
            .Take(lastCount)
            .ToListAsync(ct);

        ArgumentFalseException.ThrowIfFalse(organizations.Any(), "No organizations found.");

        var data = organizations.Select(x => x.ToResponse()).ToList();
        return new PagedResponse<GetOrganizationResponse> { TotalCount = totalCount, Data = data };
    }
}
