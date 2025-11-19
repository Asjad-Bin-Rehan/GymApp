namespace BS.Services.RedemptionService.DTOs
{
    public class AddRedemptionDTO
    {
        public int user_id { get; set; }
        public int reward_id { get; set; }
        public string? status { get; set; } = "Completed"; // Default to Completed
    }
}
