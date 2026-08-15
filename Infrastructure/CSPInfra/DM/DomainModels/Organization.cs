using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Organization : Base<string>
    {
        // Columns
        public string? Name { get; set; }
        public string? Description { get; set; }

        // FKs
        public string? TenantId { get; set; }

        // Nav
        public Tenant? Tenant { get; set; }
        public ICollection<User> Users { get; set; } = [];
        public ICollection<Venue> Venues { get; set; } = [];
    }
}