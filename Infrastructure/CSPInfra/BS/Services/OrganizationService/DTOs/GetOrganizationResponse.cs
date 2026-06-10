using Helpers.CommonModels;

namespace BS.Services.OrganizationService.DTOs
{
    public class GetOrganizationResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        // FKs
        public string? TenantId { get; set; }
    }
}
