using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Location_From_Location : Base<string>
    {
        public string? LocationId { get; set; }
        public string? OtherLocationId { get; set; }

        // Cols
        public float? Distance { get; set; }
        public int? NearbyRank { get; set; }

        // Nav
        public Location? Location { get; set; }
        public Location? OtherLocation { get; set; }
    }
}