namespace BS.Services.OrganizationService.DTOs
{
    public class UpsertOrganizationRequest
    {
        public List<UpsertOrganizationObject> Organizations { get; set; } = [];
    }

    public class UpsertOrganizationObject
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        // FKs
        public string? TenantId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpsertOrganizationResponse
    {
        public List<string> Ids { get; set; } = [];
    }
}
