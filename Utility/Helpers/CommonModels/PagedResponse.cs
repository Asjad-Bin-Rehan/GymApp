namespace Helpers.CommonModels
{
    public class PagedResponse<T>
    {
        public int TotalCount { get; set; } = 0;
        public List<T> Data { get; set; } = [];
    }
}