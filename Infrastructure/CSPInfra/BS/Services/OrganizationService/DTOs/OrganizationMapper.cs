using DM.DomainModels;

namespace BS.Services.OrganizationService.DTOs
{
    public static class OrganizationMapper
    {
        public static Organization ToInsert(this UpsertOrganizationObject request, string userId)
        {
            return new Organization
            {
                Name = request.Name,
                Description = request.Description,
                TenantId = request.TenantId,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }

        public static Organization ToUpdate(this Organization existing, UpsertOrganizationObject obj)
        {
            existing.Name = obj.Name;
            existing.Description = obj.Description;
            existing.TenantId = obj.TenantId;
            existing.IsActive = obj.IsActive;
            return existing;
        }

        public static GetOrganizationResponse ToResponse(this Organization row)
        {
            return new GetOrganizationResponse
            {
                Id = row.Id,
                Name = row.Name,
                Description = row.Description,
                TenantId = row.TenantId,
                CreatedDate = row.CreatedDate,
                CreatedBy = row.CreatedBy,
                UpdatedBy = row.UpdatedBy,
                UpdatedDate = row.UpdatedDate,
                IsActive = row.IsActive,
                IsArchived = row.IsArchived
            };
        }
    }
}
