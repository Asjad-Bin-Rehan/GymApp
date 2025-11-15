namespace BS.Services.UserService.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? MembershipId { get; set; }
        public string Status { get; set; } = "Active";
    }
}
