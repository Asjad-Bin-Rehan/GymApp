using DM.DomainModels;

namespace BS.Services.VenueService.DTOs
{
    public static class VenueMapper
    {
        public static Venue ToInsert(this UpsertVenueObject request, string userId)
        {
            return new Venue
            {
                Name = request.Name,
                Address = request.Address,
                MapUrl = request.MapUrl,
                DpUrl = request.DpUrl,
                Sports = request.Sports,
                OrganizationId = request.OrganizationId,
                LocationId = request.LocationId,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }

        public static Venue ToUpdate(this Venue existing, UpsertVenueObject obj)
        {
            existing.Name = obj.Name;
            existing.Address = obj.Address;
            existing.MapUrl = obj.MapUrl;
            existing.DpUrl = obj.DpUrl;
            existing.Sports = obj.Sports;
            existing.OrganizationId = obj.OrganizationId;
            existing.LocationId = obj.LocationId;
            existing.IsActive = obj.IsActive;
            return existing;
        }

        public static GetVenueResponse ToResponse(this Venue row)
        {
            return new GetVenueResponse
            {
                Id = row.Id,
                Name = row.Name,
                Address = row.Address,
                MapUrl = row.MapUrl,
                DpUrl = row.DpUrl,
                Sports = row.Sports,
                OrganizationId = row.OrganizationId,
                LocationId = row.LocationId,
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
