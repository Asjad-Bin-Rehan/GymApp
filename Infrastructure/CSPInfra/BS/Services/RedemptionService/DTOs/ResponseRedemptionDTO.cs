namespace BS.Services.RedemptionService.DTOs
{
    public class ResponseRedemptionDTO
    {
        public int redemption_id { get; set; }
        public int user_id { get; set; }
        public int reward_id { get; set; }
        public string status { get; set; } = null!;
        public int points_spent { get; set; }
        public DateTime redemption_date { get; set; }
    }
}
