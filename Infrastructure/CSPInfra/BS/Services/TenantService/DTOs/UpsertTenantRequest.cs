namespace BS.Services.TenantService.DTOs
{
    public class UpsertTenantRequest
    {
        public List<UpsertTenantObject> Tenants { get; set; } = [];
    }

    public class UpsertTenantObject
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpsertTenantResponse
    {
        public List<string> Ids { get; set; } = [];
    }
}
