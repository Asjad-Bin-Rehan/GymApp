namespace BS.Services.PurchaseQCService.DTOs
{
    public class ListPurchaseQCResponse
    {
        public int TotalRecords { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public int PageNumber { get; set; } = 0;
        public List<ResponsePurchaseQCWithItem> Values { get; set; } = [];
    }
}
