namespace BS.Services.LocationService.DTOs
{
    public class AddLocationDTO
    {
        public string? country { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public string? postal_code { get; set; }
        public string? address { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}
