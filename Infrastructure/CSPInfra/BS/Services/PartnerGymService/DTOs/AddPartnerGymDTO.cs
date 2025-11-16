public class AddPartnerGymDTO
{
    public string gym_name { get; set; } = null!;
    public string contact_person { get; set; } = null!;
    public string phone { get; set; } = null!;

    // Location details
    public string country { get; set; } = null!;
    public string state { get; set; } = null!;
    public string city { get; set; } = null!;
    public string postal_code { get; set; } = null!;
    public string address { get; set; } = null!;
    public double latitude { get; set; }
    public double longitude { get; set; }
}
