using DM.DomainModels;

namespace BS.Services.CourtService.DTOs
{
    public static class CourtMapper
    {
        public static Court ToInsert(this UpsertCourtObject request, string userId)
        {
            return new Court
            {
                Name = request.Name,
                Code = request.Code,
                SurfaceType = request.SurfaceType,
                VenueId = request.VenueId,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }

        public static Court ToUpdate(this Court existing, UpsertCourtObject obj)
        {
            existing.Name = obj.Name;
            existing.Code = obj.Code;
            existing.SurfaceType = obj.SurfaceType;
            existing.VenueId = obj.VenueId;
            existing.IsActive = obj.IsActive;
            return existing;
        }

        public static GetCourtResponse ToResponse(this Court row)
        {
            return new GetCourtResponse
            {
                Id = row.Id,
                Name = row.Name,
                Code = row.Code,
                SurfaceType = row.SurfaceType,
                VenueId = row.VenueId,
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
