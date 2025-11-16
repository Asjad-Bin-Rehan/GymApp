namespace BS.Services.PartnerGymService.DTOs
{
    public class UpdatePartnerGymDTO
    {
        public int gym_id { get; set; }
        public string gym_name { get; set; }
        public int? location_id { get; set; }
        public string contact_person { get; set; }
        public string phone { get; set; }
        public string status { get; set; }
    }
}
