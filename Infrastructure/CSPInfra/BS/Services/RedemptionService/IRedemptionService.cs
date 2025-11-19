using BS.Services.RedemptionService.DTOs;

namespace BS.Services.RedemptionService
{
    public interface IRedemptionService
    {
        Task<int> CreateRedemption(AddRedemptionDTO dto, CancellationToken ct);
        Task<List<ResponseRedemptionDTO>> ListAllRedemptions(CancellationToken ct);
        Task<ResponseRedemptionDTO?> GetRedemptionById(int redemption_id, CancellationToken ct);
        Task<List<ResponseRedemptionDTO>> GetRedemptionsByUserId(int user_id, CancellationToken ct);
        Task<bool> UpdateRedemption(UpdateRedemptionDTO dto, CancellationToken ct);
        Task<bool> DeleteRedemption(int redemption_id, CancellationToken ct);
    }
}
