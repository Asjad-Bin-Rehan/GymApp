namespace BS.Services.AccessLogService.DTOs
{
    public class ListAccessLogParamsDTO
    {
        public int limit { get; set; } = 100;
        public int offset { get; set; } = 0;
        // optional filters can be added later (user_id, gym_id, date range)
    }
}
