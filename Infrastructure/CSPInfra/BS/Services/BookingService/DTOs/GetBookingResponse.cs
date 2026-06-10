using Helpers.CommonModels;

namespace BS.Services.BookingService.DTOs
{
    public class GetBookingResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public string? BookingSource { get; set; }

        // FKs
        public string? SlotId { get; set; }
        public string? CustomerProfileId { get; set; }
    }
}
