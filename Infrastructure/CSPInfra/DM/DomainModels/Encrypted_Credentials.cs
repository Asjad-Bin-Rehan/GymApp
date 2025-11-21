using GenericRepository.CommonModels;

namespace DM.DomainModels
{
    public class Encrypted_Credentials : Base<string>
    {
        public string? DeviceId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserId { get; set; }
        public string? UserType { get; set; }
        public string? Hash { get; set; }
        public string? Salt { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
