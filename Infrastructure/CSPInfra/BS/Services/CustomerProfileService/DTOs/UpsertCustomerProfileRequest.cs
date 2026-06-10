namespace BS.Services.CustomerProfileService.DTOs
{
    public class UpsertCustomerProfileRequest
    {
        public List<UpsertCustomerProfileObject> CustomerProfiles { get; set; } = [];
    }

    public class UpsertCustomerProfileObject
    {
        public string? Id { get; set; }

        // FKs
        public string? UserId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpsertCustomerProfileResponse
    {
        public List<string> Ids { get; set; } = [];
    }
}
