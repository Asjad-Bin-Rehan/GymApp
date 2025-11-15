namespace BS.Services.UserService.DTOs
{
    public class ResponseUserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? MembershipId { get; set; }
        public string Status { get; set; } = "Active";
        public int TotalPoints { get; set; } = 0;

        // JWT Token
        public string? Token { get; set; }
    }
}
