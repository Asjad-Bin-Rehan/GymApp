using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Tenant : Base<string>
    {
        // Cols
        public string? Name { get; set; }

        // Nav
        public ICollection<Organization> Organizations { get; set; } = [];
    }
}