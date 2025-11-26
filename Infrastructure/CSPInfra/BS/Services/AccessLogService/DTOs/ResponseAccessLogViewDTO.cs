namespace BS.Services.AccessLogService.DTOs
{
    public class ResponseAccessLogViewDTO
    {
        public int log_id { get; set; }
        public int? user_id { get; set; }
        public string? user_name { get; set; }
        public int? gym_id { get; set; }
        public string? gym_name { get; set; }
        public DateTime? access_time { get; set; }
        public int? points_earned { get; set; }
    }
}
