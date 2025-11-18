using BS.Services.AccessLogService.DTOs;

public interface IAccessLogService
{
    Task<AccessLogResultDTO> AddAccessLogRaw(AddAccessLogDTO request, CancellationToken ct);

    Task<List<ResponseAccessLogDTO>> GetAccessLogsByUserIdRaw(int userId, CancellationToken ct);

    Task<ResponseAccessLogDTO?> GetAccessLogByIdRaw(int logId, CancellationToken ct);
    Task<List<ResponseAccessLogDTO>> ListAllAccessLogsRaw(int limit, int offset, CancellationToken ct);

    Task<bool> DeleteAccessLogRaw(int logId, CancellationToken ct);
}
