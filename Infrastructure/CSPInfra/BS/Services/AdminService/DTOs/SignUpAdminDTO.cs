namespace BS.Services.AdminService.DTOs
{
    public class SignUpAdminDTO
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
        public string role { get; set; } = "Manager"; // default role
    }
}
