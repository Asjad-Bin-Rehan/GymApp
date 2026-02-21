using DM.DomainModels;

namespace BS.Services.LocationService.DTOs
{
    public static class LocationMapper
    {
        public static Location ToInsert(this UpsertLocationObject request, string userId)
        {
            return new Location()
            {
                Name = request.Name,
                Latitude = request.Latitude,
                Longitude = request.Longitude,

                ParentId = request.ParentId,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }

        public static Location ToUpdate(this Location existing, UpsertLocationObject obj)
        {
            existing.Name = obj.Name;
            existing.Latitude = obj.Latitude;
            existing.Longitude = obj.Longitude;
            existing.ParentId = obj.ParentId;
            existing.IsActive = obj.IsActive;
            return existing;
        }

        public static GetLocationResponse ToResponse(this Location row)
        {
            return new GetLocationResponse()
            {
                Name = row.Name,
                Latitude = row.Latitude,
                Longitude = row.Longitude,

                ParentId = row.Id,

                Id = row.Id,
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