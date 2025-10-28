namespace CSAPI.Common.Auth
{
    public static class KAuthClaimTypes
    {
        public static string UserId { get; set; } = nameof(UserId);
        public static string UserType { get; set; } = nameof(UserType);
        public static string Resources { get; set; } = nameof(Resources);
        public static string Email { get; set; } = nameof(Email);
    }
}
