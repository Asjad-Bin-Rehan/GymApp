namespace BS.Services.NextIntCodeService.DTOs
{
    public class AddTestRecordsDTO
    {
        public string Password { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public int RecordsCount { get; set; } = 1;
    }
}
