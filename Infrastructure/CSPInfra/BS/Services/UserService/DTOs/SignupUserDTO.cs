namespace BS.Services.UserService.DTOs
{
    public class SignupUserDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!; // Plain password, will be hashed in service
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
