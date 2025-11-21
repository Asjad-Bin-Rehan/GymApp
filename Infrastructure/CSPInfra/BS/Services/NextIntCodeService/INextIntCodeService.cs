using BS.Services.NextIntCodeService.DTOs;

namespace BS.Services.NextIntCodeService
{
    public interface INextIntCodeService
    {
        Task<int> GetNextIntCount(string entityName, CancellationToken ct);
        Task<List<string>> ListAllEntityNamesAsync();
        Task<int> GetTotalRecordsCount(string entityName, string? filterParameter, CancellationToken ct);
        Task<bool> AddTestRecords(AddTestRecordsDTO request, string userId, CancellationToken ct);
        Task<bool> AddLogPostSap(AddLogPostSapDTO request, string userId, CancellationToken ct);
        Task<ResponseGetQualityStatus> GetQualityStatus(string entityName, string ItemCode, string? docNumber, int? lineNo, string? stageType, CancellationToken ct);
    }
}