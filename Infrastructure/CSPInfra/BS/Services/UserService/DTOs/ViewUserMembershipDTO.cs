namespace BS.Services.UserService.DTOs
{
    public class ViewUserMembershipDTO
    {
        public int user_id { get; set; }
        public string username { get; set; } = "";
        public string full_name { get; set; } = "";
        public string email { get; set; } = "";
        public string? phone { get; set; }
        public DateTime? date_of_birth { get; set; }
        public DateTime join_date { get; set; }
        public string status { get; set; } = "";
        public int total_points { get; set; }

        // Membership
        public int? subscription_id { get; set; }
        public int? plan_id { get; set; }
        public DateTime? membership_start_date { get; set; }
        public DateTime? membership_end_date { get; set; }
        public string? payment_status { get; set; }

        // Plan details
        public string? plan_name { get; set; }
        public int? duration_months { get; set; }
        public decimal? price { get; set; }
    }
}
