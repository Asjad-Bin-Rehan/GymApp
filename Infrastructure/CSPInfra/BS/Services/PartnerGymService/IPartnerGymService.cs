using BS.Services.PartnerGymService.DTOs;

namespace BS.Services.PartnerGymService
{
    public interface IPartnerGymService
    {
        Task<bool> AddPartnerGymRaw(AddPartnerGymDTO request, CancellationToken ct);
        Task<int> AddPartnerGymWithLocation(AddPartnerGymDTO request, CancellationToken ct);
        Task<ResponsePartnerGymDTO?> GetPartnerGymByIdRaw(int gymId, CancellationToken ct);
        Task<List<ResponsePartnerGymDTO>> ListPartnerGymsRaw(int limit, int offset, CancellationToken ct);
        Task<bool> UpdatePartnerGymRaw(UpdatePartnerGymDTO request, CancellationToken ct);
        Task<bool> DeletePartnerGymRaw(int gymId, CancellationToken ct);
    }
}
