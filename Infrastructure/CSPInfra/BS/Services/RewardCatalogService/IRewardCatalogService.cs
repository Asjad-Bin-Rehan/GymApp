using BS.Services.RewardCatalogService.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BS.Services.RewardCatalogService
{
    public interface IRewardCatalogService
    {
        // Create
        Task<int> AddRewardRaw(AddRewardDTO request, CancellationToken ct);

        // Read
        Task<ResponseRewardDTO?> GetRewardByIdRaw(int rewardId, CancellationToken ct);
        Task<List<ResponseRewardDTO>> ListAllRewardsRaw(CancellationToken ct);

        // Update
        Task<bool> UpdateRewardRaw(UpdateRewardDTO request, CancellationToken ct);

        // Delete
        Task<bool> DeleteRewardRaw(int rewardId, CancellationToken ct);
    }
}
