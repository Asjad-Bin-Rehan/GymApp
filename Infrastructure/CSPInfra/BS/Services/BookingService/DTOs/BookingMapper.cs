using DM.DomainModels;

namespace BS.Services.BookingService.DTOs
{
    public static class BookingMapper
    {
        public static Booking ToInsert(this UpsertBookingObject request, string userId)
        {
            return new Booking
            {
                Name = request.Name,
                Status = request.Status,
                PaymentStatus = request.PaymentStatus,
                BookingSource = request.BookingSource,
                SlotId = request.SlotId,
                CustomerProfileId = request.CustomerProfileId,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false
            };
        }

        public static Booking ToUpdate(this Booking existing, UpsertBookingObject obj)
        {
            existing.Name = obj.Name;
            existing.Status = obj.Status;
            existing.PaymentStatus = obj.PaymentStatus;
            existing.BookingSource = obj.BookingSource;
            existing.SlotId = obj.SlotId;
            existing.CustomerProfileId = obj.CustomerProfileId;
            existing.IsActive = obj.IsActive;
            return existing;
        }

        public static GetBookingResponse ToResponse(this Booking row)
        {
            return new GetBookingResponse
            {
                Id = row.Id,
                Name = row.Name,
                Status = row.Status,
                PaymentStatus = row.PaymentStatus,
                BookingSource = row.BookingSource,
                SlotId = row.SlotId,
                CustomerProfileId = row.CustomerProfileId,
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
