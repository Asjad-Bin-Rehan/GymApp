namespace BS.Services.PartnerGymService.DTOs
{
    public class UpdatePartnerGymDTO
    {
        public int gym_id { get; set; }
        public string? gym_name { get; set; }
        public string? contact_person { get; set; }
        public string? phone { get; set; }
        public string? status { get; set; }
        public int? admin_id { get; set; }

        // Add this so service can set it internally
        public int? location_id { get; set; }

        // New location fields
        public string? country { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public string? postal_code { get; set; }
        public string? address { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}
