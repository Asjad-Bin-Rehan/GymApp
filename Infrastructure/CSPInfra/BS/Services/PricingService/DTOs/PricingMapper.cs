using DM.DomainModels;

namespace BS.Services.PricingService.DTOs
{
    public static class PricingMapper
    {
        public static Pricing ToInsert(this UpsertPricingObject request, string userId)
        {
            return new Pricing
            {
                Day = request.Day,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Price = request.Price,
                CourtId = request.CourtId,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }

        public static Pricing ToUpdate(this Pricing existing, UpsertPricingObject obj)
        {
            existing.Day = obj.Day;
            existing.StartTime = obj.StartTime;
            existing.EndTime = obj.EndTime;
            existing.Price = obj.Price;
            existing.CourtId = obj.CourtId;
            existing.IsActive = obj.IsActive;
            return existing;
        }

        public static GetPricingResponse ToResponse(this Pricing row)
        {
            return new GetPricingResponse
            {
                Id = row.Id,
                Day = row.Day,
                StartTime = row.StartTime,
                EndTime = row.EndTime,
                Price = row.Price,
                CourtId = row.CourtId,
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
