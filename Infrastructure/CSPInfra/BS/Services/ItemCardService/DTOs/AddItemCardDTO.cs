namespace BS.Services.ItemCardService.DTOs
{
    public class AddItemCardDTO
    {
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
        public bool IsActive { get; set; } = true;
    }
}