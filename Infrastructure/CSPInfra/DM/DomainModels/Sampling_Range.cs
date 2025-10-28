using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Sampling_Range : Base<string>
    {
        // Columns
        public int IntCode { get; set; }
        public double? LotSizeMin { get; set; }
        public double? LotSizeMax { get; set; }
        public double? SampleQty { get; set; }
        public double? CriticalDefects { get; set; }
        public double? MajorDefects { get; set; }
        public double? MinorDefects { get; set; }
        
        // FKs
        public string? ItemSampleId { get; set; }

        // Navigation
        public Item_Sample? Item_Sample { get; set; }
        public ICollection<Log_Sampling_Range> Log_Sampling_Ranges { get; set; } = [];
    }
}
