namespace BS.Services.PointsHistoryService.DTOs
{
    public class AddPointsHistoryDTO
    {
        public int user_id { get; set; }
        public int points_change { get; set; }
        public string? reason { get; set; }
    }
}
