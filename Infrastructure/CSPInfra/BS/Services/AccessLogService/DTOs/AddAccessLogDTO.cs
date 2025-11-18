namespace BS.Services.AccessLogService.DTOs
{
    public class AddAccessLogDTO
    {
        public int user_id { get; set; }
        public int gym_id { get; set; }

        public double device_lat { get; set; }
        public double device_lon { get; set; }
    }
}
