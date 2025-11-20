namespace BS.Services.AdminService.DTOs
{
    public class AdminDTO
    {
        public int admin_id { get; set; }
        public string username { get; set; } = null!;
        public string role { get; set; } = null!;
        public string full_name { get; set; } = null!;
        public string? phone { get; set; }
        public DateTime? date_of_birth { get; set; }
        public DateTime join_date { get; set; }
        public string email { get; set; } = null!;
    }
}
