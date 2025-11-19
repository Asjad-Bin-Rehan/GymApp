namespace BS.Services.AdminService.DTOs
{
    public class LoginAdminWithoutJWTDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginAdminWithoutJWTResponseDTO
    {
        public int AdminId { get; set; }
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
