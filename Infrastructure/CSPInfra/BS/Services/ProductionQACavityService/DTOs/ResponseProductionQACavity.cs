using Helpers.CommonModels;

namespace BS.Services.ProductionQACavityService.DTOs
{
    public class ResponseProductionQACavity : ActivityTrackersInResponse
    {
        // UI: Metadata
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? InspectionBy { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public bool? IsCavityPassed { get; set; }

        // UI: Header
        public string? MouldNo { get; set; }
        public bool? IsToggledOn { get; set; }

        // FK
        public string? QaId { get; set; }
    }
}