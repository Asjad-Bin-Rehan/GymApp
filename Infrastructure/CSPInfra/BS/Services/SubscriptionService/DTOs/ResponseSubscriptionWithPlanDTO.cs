namespace BS.Services.SubscriptionService.DTOs
{
    public class ResponseSubscriptionWithPlanDTO
    {
        public int subscription_id { get; set; }
        public int user_id { get; set; }
        public int plan_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string payment_status { get; set; } = "Paid";
        
        // Membership Plan details
        public string plan_name { get; set; } = string.Empty;
        public int duration_months { get; set; }
        public decimal price { get; set; }
        public string description { get; set; } = string.Empty;
    }
}
