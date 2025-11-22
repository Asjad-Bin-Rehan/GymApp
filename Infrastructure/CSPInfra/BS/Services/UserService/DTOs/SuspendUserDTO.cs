namespace BS.Services.UserService.DTOs
{
    public class SuspendUserDTO
    {
        public int user_id { get; set; } // User to suspend
        public int admin_id { get; set; } // The admin performing the action
    }
}
