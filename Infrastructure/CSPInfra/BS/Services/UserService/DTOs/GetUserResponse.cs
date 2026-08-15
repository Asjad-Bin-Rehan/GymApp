using Helpers.CommonModels;

namespace BS.Services.UserService.DTOs
{
    public class GetUserResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserType { get; set; }

        // FKs
        public string? OrganizationId { get; set; }
    }
}
