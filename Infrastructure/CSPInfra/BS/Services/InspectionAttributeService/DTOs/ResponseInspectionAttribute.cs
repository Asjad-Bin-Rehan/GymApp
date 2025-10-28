using Helpers.CommonModels;

namespace BS.Services.InspectionAttributeService.DTOs
{
    public class ResponseInspectionAttribute : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
    }
}
