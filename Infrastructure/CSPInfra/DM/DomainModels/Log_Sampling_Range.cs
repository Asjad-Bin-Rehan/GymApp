using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Log_Sampling_Range : Base<string>
    {
        public double? LotSizeMin { get; set; }
        public double? LotSizeMax { get; set; }
        public double? SampleQty { get; set; }
        public double? CriticalDefects { get; set; }
        public double? MajorDefects { get; set; }
        public double? MinorDefects { get; set; }
        public bool? ItemSampleFlexibility { get; set; }

        // FKs
        public string? ItemSampleId { get; set; }

        // Log-FK
        public string? SamplingRangeId { get; set; }

        // Navigation
        public Sampling_Range? SamplingRange { get; set; }
    }
}
