using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Location : Base<string>
    {
        public string? Name { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }

        // FK
        public string? ParentId { get; set; }

        // Nav
        public Location? LocationParent { get; set; }
        public ICollection<Location> LocationChildren { get; set; } = [];
        public ICollection<Location_From_Location> Location_From_Location { get; set; } = [];
        public ICollection<Location_From_Location> Other_Location_From_Location { get; set; } = [];
        public ICollection<Venue> Venues { get; set; } = [];
    }
}