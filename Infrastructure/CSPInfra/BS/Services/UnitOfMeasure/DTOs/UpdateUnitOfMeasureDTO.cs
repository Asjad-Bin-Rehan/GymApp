namespace BS.Services.UnitOfMeasure.DTOs
{
    public class UpdateUnitOfMeasureDTO
    {
        public string Id { get; set; }
        public string? UoMCode { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
