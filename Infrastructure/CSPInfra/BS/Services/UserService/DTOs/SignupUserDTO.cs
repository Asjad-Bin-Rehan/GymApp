namespace BS.Services.UserService.DTOs
{
    public class SignupUserDTO
    {
        public string? username { get; set; }
        public string? password { get; set; }
        public string? full_name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public DateTime? date_of_birth { get; set; }

        // NEW: membership plan selected from frontend
        public int plan_id { get; set; }
    }
}
