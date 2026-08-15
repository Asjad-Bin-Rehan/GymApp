namespace BS.Services.UserService.DTOs
{
    public class UpsertUserRequest
    {
        public List<UpsertUserObject> Users { get; set; } = [];
    }

    public class UpsertUserObject
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserType { get; set; }

        // FKs
        public string? OrganizationId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpsertUserResponse
    {
        public List<string> Ids { get; set; } = [];
    }
}
