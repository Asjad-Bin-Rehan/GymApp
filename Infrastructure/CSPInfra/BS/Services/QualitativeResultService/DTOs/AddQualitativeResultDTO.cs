using System.ComponentModel.DataAnnotations;

namespace BS.Services.QualitativeResultService.DTOs;

public class AddQualitativeResultDTO
{
    [Required]
    public string? ResultDescription { get; set; }
    public bool IsActive { get; set; } = true;
}