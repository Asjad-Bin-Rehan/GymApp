namespace BS.Services.UserService.DTOs
{
    public class LoginUserDTO
    {
        public string UsernameOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
