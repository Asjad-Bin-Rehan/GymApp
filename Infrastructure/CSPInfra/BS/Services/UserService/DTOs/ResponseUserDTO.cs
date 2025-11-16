namespace BS.Services.UserService.DTOs
{
    public class ResponseUserDTO
    {
        public int user_id { get; set; }
        public string username { get; set; } = null!;
        public string full_name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string? phone { get; set; }
        public string? membership_id { get; set; }
        public string status { get; set; } = "Active";
        public int total_points { get; set; } = 0;
        public DateTime? date_of_birth { get; set; }
        public DateTime? join_date { get; set; }

        // JWT Token (optional)
        // public string? token { get; set; }
    }
}
