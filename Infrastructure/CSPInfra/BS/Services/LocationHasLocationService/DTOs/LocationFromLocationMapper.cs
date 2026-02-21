using DM.DomainModels;

namespace BS.Services.LocationHasLocationService.DTOs
{
    public static class LocationFromLocationMapper
    {
        public static Location_From_Location ToInsert(this UpsertLocationFromLocationObject obj, string userId)
        {
            return new Location_From_Location
            {
                Id = Guid.NewGuid().ToString(),
                LocationId = obj.LocationId,
                OtherLocationId = obj.OtherLocationId,
                Distance = obj.Distance,
                NearbyRank = obj.Distance <= 10000 ? 1 : 0,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static void ToUpdate(this Location_From_Location existing, UpsertLocationFromLocationObject obj)
        {
            existing.NearbyRank = obj.NearbyRank;
            existing.Distance = obj.Distance;
            existing.IsActive = obj.IsActive;
        }

        public static GetLocationFromLocationResponse ToResponse(this Location_From_Location row)
        {
            return new GetLocationFromLocationResponse
            {
                Id = row.Id,
                LocationId = row.LocationId,
                OtherLocationId = row.OtherLocationId,
                Distance = row.Distance,
                NearbyRank = row.NearbyRank,
            };
        }
    }
}