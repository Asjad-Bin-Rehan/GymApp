namespace BS.Services.PointsHistoryService.DTOs
{
    public class UpdatePointsHistoryDTO
    {
        public int points_id { get; set; }
        public int? points_change { get; set; }
        public string? reason { get; set; }
    }
}
