namespace BS.Services.UnitOfMeasure.DTOs
{
    public class AddUnitOfMeasureDTO
    {
        public string? Description { get; set; }
        public string? UoMCode { get; set; }
        public bool IsActive { get; set; } = true;
    }
}