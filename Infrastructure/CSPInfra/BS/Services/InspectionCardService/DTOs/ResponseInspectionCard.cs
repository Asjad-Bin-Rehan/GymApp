using Helpers.CommonModels;

namespace BS.Services.InspectionCardService.DTOs
{
    public class ResponseInspectionCard : ActivityTrackersInResponse
    {
        public string Id { get; set; } = string.Empty;
        public int IntCode { get; set; }
        public string? Description { get; set; }
    }
}
