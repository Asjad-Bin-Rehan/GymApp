namespace BS.Services.MembershipPlanService.DTOs
{
    public class AddMembershipPlanDTO
    {
        public string? plan_name { get; set; }
        public int duration_months { get; set; }
        public decimal price { get; set; }
        public string? description { get; set; }
    }
}
