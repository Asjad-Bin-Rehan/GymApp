namespace BS.Services.PartnerGymService.DTOs
{
    public class ResponsePartnerGymDTO
    {
        public int gym_id { get; set; }
        public string gym_name { get; set; }
        public int? location_id { get; set; }
        public string contact_person { get; set; }
        public string phone { get; set; }
        public DateTime? partnership_date { get; set; }
        public string status { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public string address { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }
}
