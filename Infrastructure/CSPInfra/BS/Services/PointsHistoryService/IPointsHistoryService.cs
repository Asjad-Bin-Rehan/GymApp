using BS.Services.PointsHistoryService.DTOs;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BS.Services.PointsHistoryService
{
    public interface IPointsHistoryService
    {
        // CREATE
        Task<int> AddPointsHistory(AddPointsHistoryDTO dto, CancellationToken ct);

        // READ
        Task<ResponsePointsHistoryDTO?> GetPointsHistoryById(int pointsId, CancellationToken ct);
        Task<List<ResponsePointsHistoryDTO>> ListAllPointsHistory(CancellationToken ct);
        Task<List<ResponsePointsHistoryDTO>> GetPointsHistoryByUserId(int userId, CancellationToken ct);

        // UPDATE
        Task<bool> UpdatePointsHistory(UpdatePointsHistoryDTO dto, CancellationToken ct);

        // DELETE
        Task<bool> DeletePointsHistory(int pointsId, CancellationToken ct);
    }
}
