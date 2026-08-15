using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Slot : Base<string>
    {
        // Cols
        public string? Day { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool? IsAvailable { get; set; }
        public float? CalculatedPrice { get; set; }     // performant & auditable - doesn't change when pricing changed hence no pricing FK

        // FKs
        public string? CourtId { get; set; }

        // Nav
        public Court? Court { get; set; }
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}