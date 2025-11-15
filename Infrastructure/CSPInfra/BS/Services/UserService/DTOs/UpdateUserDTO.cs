namespace BS.Services.UserService.DTOs
{
    public class UpdateUserDTO
    {
        public int UserId { get; set; }                // Required for update
        public string? Username { get; set; }          // Optional
        public string? FullName { get; set; }          // Optional
        public string? Email { get; set; }             // Optional
        public string? Phone { get; set; }             // Optional
        public string? MembershipId { get; set; }      // Optional
        public string? Status { get; set; }            // Optional: Active, Inactive, Expired
    }
}
