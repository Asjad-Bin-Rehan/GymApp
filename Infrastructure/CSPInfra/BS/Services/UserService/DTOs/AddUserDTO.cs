namespace BS.Services.UserService.DTOs
{
    public class AddUserDTO
    {
        public string? username { get; set; }
        public string? password { get; set; }
        public string? full_name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public DateTime? date_of_birth { get; set; }
        public string? membership_id { get; set; }
        public string? status { get; set; } = "Active";
        public int total_points { get; set; } = 0;
    }
}
