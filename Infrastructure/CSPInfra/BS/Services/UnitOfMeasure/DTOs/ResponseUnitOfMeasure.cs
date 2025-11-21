using Helpers.CommonModels;

namespace BS.Services.UnitOfMeasure.DTOs
{
    public class ResponseUnitOfMeasure : ActivityTrackersInResponse
    {
        public int IntCode { get; set; }
        public string? Id { get; set; }
        public string? UoMCode { get; set; }
        public string? Description { get; set; }
    }
}
