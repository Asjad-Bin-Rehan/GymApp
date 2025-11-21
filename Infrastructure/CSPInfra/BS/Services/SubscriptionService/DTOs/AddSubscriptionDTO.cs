namespace BS.Services.SubscriptionService.DTOs
{
    public class AddSubscriptionDTO
    {
        public int user_id { get; set; }
        public int plan_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string payment_status { get; set; } = "Paid";
    }
}
