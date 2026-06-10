using DM.DomainModels;

namespace BS.Services.TenantService.DTOs
{
    public static class TenantMapper
    {
        public static Tenant ToInsert(this UpsertTenantObject request, string userId)
        {
            return new Tenant
            {
                Name = request.Name,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }

        public static Tenant ToUpdate(this Tenant existing, UpsertTenantObject obj)
        {
            existing.Name = obj.Name;
            existing.IsActive = obj.IsActive;
            return existing;
        }

        public static GetTenantResponse ToResponse(this Tenant row)
        {
            return new GetTenantResponse
            {
                Id = row.Id,
                Name = row.Name,
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
