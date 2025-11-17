namespace BS.Services.AccessLogService.DTOs
{
    public class ResponseAccessLogDTO
    {
        public int log_id { get; set; }
        public int? user_id { get; set; }
        public int? gym_id { get; set; }
        public DateTime? access_time { get; set; }
        public int? points_earned { get; set; }
    }
}
