using BS.Services.QualitativeResultService.DTOs;

namespace BS.Services.QualitativeResultService;

public interface IQualitativeResultService
{
    Task<bool> AddQualitativeResult(AddQualitativeResultDTO request, string userId, CancellationToken ct);
    Task<List<ResponseQualitativeResult>> ListAllQualitativeResults(CancellationToken cancellationToken, int lastCount, int skipRecords);
    Task<bool> UpdateQualitativeResult(UpdateQualitativeResultDTO request, string userId, CancellationToken ct);
    Task<List<ResponseQualitativeResult>> GetQualitativeResultById(string id, CancellationToken cancellationToken);

    // Custom FluentValidations
    Task<bool> IsQualitativeResultUsed(string Id, CancellationToken ct);
    Task<bool> IsQualitativeResultDescriptionExists(string? resultDescription, CancellationToken ct);

}