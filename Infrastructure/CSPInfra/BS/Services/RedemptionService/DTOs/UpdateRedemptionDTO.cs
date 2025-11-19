namespace BS.Services.RedemptionService.DTOs
{
    public class UpdateRedemptionDTO
    {
        public int redemption_id { get; set; }
        public string status { get; set; } = null!;
    }
}
