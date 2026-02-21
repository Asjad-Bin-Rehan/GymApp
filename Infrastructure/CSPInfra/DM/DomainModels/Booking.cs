using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Booking : Base<string>
    {
        // Cols
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public string? BookingSource { get; set; }      // App / Walk-in

        // FKs
        public string? SlotId { get; set; }
        public string? CustomerProfileId { get; set; }

        // Nav
        public Slot? Slot { get; set; }
        public CustomerProfile? CustomerProfile { get; set; }
    }
}