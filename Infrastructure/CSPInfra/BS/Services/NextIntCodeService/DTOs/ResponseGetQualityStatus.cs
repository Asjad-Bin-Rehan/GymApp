namespace BS.Services.NextIntCodeService.DTOs
{
    public class ResponseGetQualityStatus
    {
        public bool? IsPerformed { get; set; }
        public bool? IsPostedToSap { get; set; }
        public bool? IsClosed { get; set; }
        public bool? OverallStatus { get; set; }
    }
}
