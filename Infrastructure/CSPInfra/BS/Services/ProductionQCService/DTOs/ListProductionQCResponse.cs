namespace BS.Services.ProductionQCService.DTOs
{
    public class ListProductionQCResponse
    {
        public int TotalRecords { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public int PageNumber { get; set; } = 0;
        public List<ResponseProductionQCWithItem> Values { get; set; } = [];
    }
}
