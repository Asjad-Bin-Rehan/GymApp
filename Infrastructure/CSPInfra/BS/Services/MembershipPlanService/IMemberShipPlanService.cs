using BS.Services.MembershipPlanService.DTOs;

namespace BS.Services.MembershipPlanService
{
    public interface IMembershipPlanService
    {
        // Create
        Task<bool> AddMembershipPlanRaw(AddMembershipPlanDTO request, CancellationToken ct);

        // Read
        Task<ResponseMembershipPlanDTO?> GetMembershipPlanByIdRaw(int planId, CancellationToken ct);
        Task<List<ResponseMembershipPlanDTO>> ListAllMembershipPlansRaw(CancellationToken ct);

        // Update
        Task<bool> UpdateMembershipPlanRaw(UpdateMembershipPlanDTO request, CancellationToken ct);

        // Delete
        Task<bool> DeleteMembershipPlanRaw(int planId, CancellationToken ct);
    }
}
