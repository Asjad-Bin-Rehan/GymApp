namespace BS.Services.PointsHistoryService.DTOs
{
    public class ResponsePointsHistoryDTO
    {
        public int points_id { get; set; }
        public int user_id { get; set; }
        public int points_change { get; set; }
        public string? reason { get; set; }
        public DateTime change_date { get; set; }
    }
}
