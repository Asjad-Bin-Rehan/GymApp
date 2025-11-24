using BS.Services.SubscriptionService.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static BS.Services.SubscriptionService.SubscriptionService;

namespace BS.Services.SubscriptionService
{
    public interface ISubscriptionService
    {
        // CREATE
        Task<int> AddSubscription(AddSubscriptionDTO dto, CancellationToken ct);

        // READ
        Task<ResponseSubscriptionDTO?> GetSubscriptionById(int subscriptionId, CancellationToken ct);
        Task<List<ResponseSubscriptionDTO>> ListAllSubscriptions(CancellationToken ct);
        Task<ResponseSubscriptionWithPlanDTO?> GetSubscriptionByUserId(int userId, CancellationToken ct);

        // UPDATE
        Task<bool> UpdateSubscription(UpdateSubscriptionDTO dto, CancellationToken ct);

        // DELETE
        Task<bool> DeleteSubscription(int subscriptionId, CancellationToken ct);

        Task<RenewResult> RenewOrUpgradeAsync(RenewSubscriptionDTO request, CancellationToken ct);
    }
}
