namespace BS.Services.ItemSampleService.DTO;

public class AddItemSampleWithRangesDTO
{
    public string? ItemId { get; set; }
    public string? ItemDescription { get; set; }
    public bool? Flexibility { get; set; }
    public bool IsActive { get; set; } = true;
    public List<SamplingRangeContract> SamplingRangeObjects { get; set; } = [];
    
    public class SamplingRangeContract
    {
        public int? LotSizeMin { get; set; }
        public int? LotSizeMax { get; set; }
        public int? SampleQty { get; set; }
        public int? CriticalDefects { get; set; }
        public int? MajorDefects { get; set; }
        public int? MinorDefects { get; set; }
    }
}