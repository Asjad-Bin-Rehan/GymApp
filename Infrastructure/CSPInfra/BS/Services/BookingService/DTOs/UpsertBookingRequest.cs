namespace BS.Services.BookingService.DTOs
{
    public class UpsertBookingRequest
    {
        public List<UpsertBookingObject> Bookings { get; set; } = [];
    }

    public class UpsertBookingObject
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public string? BookingSource { get; set; }

        // FKs
        public string? SlotId { get; set; }
        public string? CustomerProfileId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpsertBookingResponse
    {
        public List<string> Ids { get; set; } = [];
    }
}
