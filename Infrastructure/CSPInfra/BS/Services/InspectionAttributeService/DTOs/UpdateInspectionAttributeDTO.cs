namespace BS.Services.InspectionAttributeService.DTOs
{
    public class UpdateInspectionAttributeDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
        public bool IsActive { get; set; } = true;
    }
}