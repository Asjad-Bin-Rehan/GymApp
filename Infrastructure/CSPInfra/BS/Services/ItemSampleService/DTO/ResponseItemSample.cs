using Helpers.CommonModels;

namespace BS.Services.ItemSampleService.DTO;

public class ResponseItemSample : ActivityTrackersInResponse
{
    // Item Sample
    public string Id { get; set; } = string.Empty;
    public int IntCode { get; set; }
    public string? ItemDescription { get; set; }
    public bool? Flexibility { get; set; }
    public string? ItemId { get; set; }

    // Item
    public string? ItemCode { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? GroupCode { get; set; }
    public string? U_QACard { get; set; }
    public string? UoMGroupEntry { get; set; }
    public bool? IsEnabledForQA { get; set; }
    public string? GroupName { get; set; }
    public string? ManageBatchNumbers { get; set; }
    public bool? IsBatch { get; set; }
    public double? PackSize { get; set; }
}