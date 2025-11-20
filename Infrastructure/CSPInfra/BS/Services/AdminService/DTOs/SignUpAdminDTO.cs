namespace BS.Services.AdminService.DTOs
{
    public class SignUpAdminDTO
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
        public string role { get; set; } = "GymTeam";
        public string full_name { get; set; } = "Unknown";
        public string? phone { get; set; }
        public DateTime? date_of_birth { get; set; }
        public string email { get; set; } = null!;
    }

}
