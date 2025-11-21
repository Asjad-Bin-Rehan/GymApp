using Helpers.CommonModels;

namespace BS.Services.ItemSampleService.DTO;

public class ResponseItemSampleWithSamplingRanges : ActivityTrackersInResponse
{
    public string Id { get; set; } = string.Empty;
    public int? IntCode { get; set; }
    public string? ItemDescription { get; set; }
    public bool? Flexibility { get; set; }
    public string? ItemId { get; set; }

    public List<SamplingRangeObject> SamplingRangeObjects { get; set; } = [];
    public class SamplingRangeObject
    {
        public string? Id { get; set; }
        public double? LotSizeMin { get; set; }
        public double? LotSizeMax { get; set; }
        public double? SampleQty { get; set; }
        public double? CriticalDefects { get; set; }
        public double? MajorDefects { get; set; }
        public double? MinorDefects { get; set; }
        public bool IsActive { get; set; }
    }
}