namespace BS.Services.UserService.DTOs
{
    public class AddUserDTO
    {
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }   // Store hashed password
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? MembershipId { get; set; }
        public string? Status { get; set; } = "Active";
        public int TotalPoints { get; set; } = 0;
    }
}
