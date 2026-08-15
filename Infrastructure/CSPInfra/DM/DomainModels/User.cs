using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class User : Base<string>
    {
        // Cols
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserType { get; set; }

        // Temporary
        public string? InitialPasswordHash { get; set; }
        public string? InitialPasswordSalt { get; set; }
        public string? InitialToken { get; set; }
        public string? InitialRefreshToken { get; set; }

        // FKs
        public string? OrganizationId { get; set; }

        // Nav
        public Organization? Organization { get; set; }
        public ICollection<CustomerProfile> CustomerProfiles { get; set; } = [];
    }
}