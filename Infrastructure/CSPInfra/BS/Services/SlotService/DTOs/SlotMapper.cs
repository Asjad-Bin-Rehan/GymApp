using DM.DomainModels;

namespace BS.Services.SlotService.DTOs
{
    public static class SlotMapper
    {
        public static Slot ToInsert(this UpsertSlotObject request, string userId)
        {
            return new Slot
            {
                Day = request.Day,
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                IsAvailable = request.IsAvailable,
                CalculatedPrice = request.CalculatedPrice,
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

        public static Slot ToUpdate(this Slot existing, UpsertSlotObject obj)
        {
            existing.Day = obj.Day;
            existing.StartDateTime = obj.StartDateTime;
            existing.EndDateTime = obj.EndDateTime;
            existing.IsAvailable = obj.IsAvailable;
            existing.CalculatedPrice = obj.CalculatedPrice;
            existing.CourtId = obj.CourtId;
            existing.IsActive = obj.IsActive;
            return existing;
        }

        public static GetSlotResponse ToResponse(this Slot row)
        {
            return new GetSlotResponse
            {
                Id = row.Id,
                Day = row.Day,
                StartDateTime = row.StartDateTime,
                EndDateTime = row.EndDateTime,
                IsAvailable = row.IsAvailable,
                CalculatedPrice = row.CalculatedPrice,
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
