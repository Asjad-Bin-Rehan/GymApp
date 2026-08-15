using Helpers.CommonModels;

namespace BS.Services.TenantService.DTOs
{
    public class GetTenantResponse : ActivityTrackersInResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
