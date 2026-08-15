using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Pricing : Base<string>
    {
        // Cols
        public string? Day { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public float? Price { get; set; }

        // FKs
        public string? CourtId { get; set; }

        // Nav
        public Court? Court { get; set; }
    }
}