namespace BS.Services.PartnerGymService.DTOs
{
    public class RawPartnerGymDTO
    {
        public int gym_id { get; set; }
        public string gym_name { get; set; } = string.Empty;
        public int? location_id { get; set; }
        public string? contact_person { get; set; }
        public string? phone { get; set; }
        public DateTime? partnership_date { get; set; }
        public string status { get; set; } = string.Empty;

        // Location details
        public string? country { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public string? postal_code { get; set; }
        public string? address { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}
