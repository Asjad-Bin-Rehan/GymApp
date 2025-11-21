using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Inspection_Attribute : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }

        // Navigation
        public ICollection<Inspection_Characteristic> Inspection_Characteristics { get; set; } = [];
    }
}
