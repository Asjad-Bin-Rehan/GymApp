namespace BS.Services.PartnerGymService.DTOs
{
    public class SearchPartnerGymRequestDTO
    {
        public int? gym_id { get; set; }
        public string? name { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
    }
}
