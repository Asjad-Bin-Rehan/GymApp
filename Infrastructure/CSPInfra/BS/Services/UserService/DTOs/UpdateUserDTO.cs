namespace BS.Services.UserService.DTOs
{
    public class UpdateUserDTO
    {
        public int user_id { get; set; }                // Required for update
        public string? username { get; set; }          // Optional
        public string? full_name { get; set; }         // Optional
        public string? email { get; set; }             // Optional
        public string? phone { get; set; }             // Optional
        public string? membership_id { get; set; }     // Optional
        public string? status { get; set; }            // Optional: Active, Inactive, Expired
    }
}
