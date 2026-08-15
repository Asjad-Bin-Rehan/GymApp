using BS.Services.BookingService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.BookingService
{
    public interface IBookingService
    {
        Task<UpsertBookingResponse> UpsertBooking(UpsertBookingRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetBookingResponse>> GetBooking(string? bookingId, string? slotId, string? customerProfileId, string? status, string? paymentStatus, string? name, int lastCount, int skipRecords, CancellationToken ct);
    }
}
