namespace BS.Services.MembershipPlanService.DTOs
{
    public class ResponseMembershipPlanDTO
    {
        public int plan_id { get; set; }
        public string plan_name { get; set; } = string.Empty;
        public int duration_months { get; set; }
        public decimal price { get; set; }
        public string description { get; set; } = string.Empty;
    }
}
