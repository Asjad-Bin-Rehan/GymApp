namespace BS.Services.AccessLogService.DTOs
{
    public class AddAccessLogDTO
    {
        public int? user_id { get; set; }            // required
        public int? gym_id { get; set; }             // required
        public DateTime? access_time { get; set; }   // optional; DB default CURRENT_TIMESTAMP if null
        public int? points_earned { get; set; }      // optional; DB default 10 if null
    }
}
