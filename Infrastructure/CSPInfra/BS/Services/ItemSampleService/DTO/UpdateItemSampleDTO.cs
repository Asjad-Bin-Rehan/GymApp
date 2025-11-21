namespace BS.Services.ItemSampleService.DTO
{
    public class UpdateItemSampleDTO
    {
        public string Id { get; set; }
        public bool? Flexibility { get; set; }
        public List<AttachSamplingRangeObject> SamplingRangeObjects { get; set; } = [];
    }

    public class AttachSamplingRangeObject
    {
        public string? Id { get; set; }             // IF sent NULL then Add ELSE Update
        public int? LotSizeMin { get; set; }
        public int? LotSizeMax { get; set; }
        public int? SampleQty { get; set; }
        public int? CriticalDefects { get; set; }
        public int? MajorDefects { get; set; }
        public int? MinorDefects { get; set; }
        public bool IsActive { get; set; } = true; // IF sent false THEN it'll detach automatically
    }
}