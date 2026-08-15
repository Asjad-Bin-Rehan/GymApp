using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Venue : Base<string>
    {
        // Columns
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? MapUrl { get; set; }
        public string? DpUrl { get; set; }
        public string? Sports { get; set; }

        // FKs
        public string? OrganizationId { get; set; }
        public string? LocationId { get; set; }

        // Navigation
        public Organization? Organization { get; set; }
        public Location? Location { get; set; }
        public ICollection<Court> Courts { get; set; } = [];
    }
}