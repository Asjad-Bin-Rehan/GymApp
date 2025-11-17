using BS.Services.AccessLogService.DTOs;

namespace BS.Services.AccessLogService
{
    public interface IAccessLogService
    {
        // Create (RAW)
        Task<bool> AddAccessLogRaw(AddAccessLogDTO request, CancellationToken ct);

        Task<List<ResponseAccessLogDTO>> GetAccessLogsByUserIdRaw(int userId, CancellationToken ct);

        // Read
        Task<ResponseAccessLogDTO?> GetAccessLogByIdRaw(int logId, CancellationToken ct);
        Task<List<ResponseAccessLogDTO>> ListAllAccessLogsRaw(int limit, int offset, CancellationToken ct);

        // Delete (RAW)
        Task<bool> DeleteAccessLogRaw(int logId, CancellationToken ct);
    }
}
