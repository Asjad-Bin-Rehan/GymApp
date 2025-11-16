namespace BS.Services.UserService.DTOs
{
    public class SignupUserDTO
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
        public string full_name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string? phone { get; set; }
        public DateTime? date_of_birth { get; set; }
    }
}
