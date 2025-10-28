using Helpers.CommonModels;

namespace BS.Services.QualitativeResultService.DTOs;

public class ResponseQualitativeResult : ActivityTrackersInResponse
{
    public string Id { get; set; } = string.Empty;
    public int IntCode { get; set; }
    public string? ResultDescription { get; set; }
}