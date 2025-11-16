namespace BS.Services.UserService.DTOs
{
    public class LoginUserDTO
    {
        public string username_or_email { get; set; } = null!;
        public string password { get; set; } = null!;
    }
}
