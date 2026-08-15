using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Court : Base<string>
    {
        // Cols
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? SurfaceType { get; set; }

        // FKs
        public string? VenueId { get; set; }

        // Nav
        public Venue? Venue { get; set; }
        public ICollection<Slot> Slots { get; set; } = [];
        public ICollection<Pricing> Pricings { get; set; } = [];
    }
}