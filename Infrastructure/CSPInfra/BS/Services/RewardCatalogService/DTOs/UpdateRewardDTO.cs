namespace BS.Services.RewardCatalogService.DTOs
{
    public class UpdateRewardDTO
    {
        public int reward_id { get; set; }
        public string reward_name { get; set; } = null!;
        public string? description { get; set; }
        public int points_cost { get; set; }
        public string? category { get; set; }
        public string? active_status { get; set; } = "Y";
    }
}
