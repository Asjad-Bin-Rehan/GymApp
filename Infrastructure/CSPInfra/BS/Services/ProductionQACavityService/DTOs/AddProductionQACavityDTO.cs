namespace BS.Services.ProductionQACavityService.DTOs
{
    public class AddProductionQACavityDTO
    {
        // UI: Metadata
        public int CavityNum { get; set; } = 1;
        public string? Name { get; set; }
        public string? InspectionBy { get; set; }
        public DateTime? InspectionDateTime { get; set; }
        public bool? IsCavityPassed { get; set; }
        public bool IsActive { get; set; } = true;

        // UI: Header
        public string? MouldNo { get; set; }
        public bool? IsToggledOn { get; set; }

        // FK
        public string? QaId { get; set; }
    }
}