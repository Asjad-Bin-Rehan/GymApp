namespace BS.Services.QualitativeResultService.DTOs;

public class UpdateQualitativeResultDTO
{
    public string Id { get; set; }
    public string? ResultDescription { get; set; }
    public bool IsActive { get; set; } = true;
}