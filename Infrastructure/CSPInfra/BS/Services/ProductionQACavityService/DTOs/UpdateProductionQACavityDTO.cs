namespace BS.Services.ProductionQACavityService.DTOs
{
    public class UpdateProductionQACavityDTO
    {
        public string Id { get; set; }

        // UI: Metadata
        public string? Name { get; set; }
        public string? InspectionBy { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public bool IsActive { get; set; } = true;
        public bool? IsCavityPassed { get; set; }

        // UI: Header
        public string? MouldNo { get; set; }
        public bool? IsToggledOn { get; set; }
    }
}