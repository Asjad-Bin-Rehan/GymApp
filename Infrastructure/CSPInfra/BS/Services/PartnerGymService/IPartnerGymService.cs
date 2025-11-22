using BS.Services.PartnerGymService.DTOs;

namespace BS.Services.PartnerGymService
{
    public interface IPartnerGymService
    {
        Task<int> AddPartnerGymRaw(AddPartnerGymDTO request, int adminId, CancellationToken ct);

        Task<int> AddPartnerGymManual(AddPartnerGymManualDTO request, CancellationToken ct);
        Task<ResponsePartnerGymDTO?> GetPartnerGymByIdRaw(int gymId, CancellationToken ct);
        Task<List<ResponsePartnerGymByAdminDTO>> GetPartnerGymsByAdminIdRaw(int adminId, CancellationToken ct);

        Task<List<ResponsePartnerGymDTO>> ListPartnerGymsRaw(int limit, int offset, CancellationToken ct);
        Task<List<ActiveGymDTO>> ListActivePartnerGymsRaw(int limit, int offset, CancellationToken ct);
        Task<bool> UpdatePartnerGymRaw(UpdatePartnerGymDTO request, CancellationToken ct);
        Task<bool> DeletePartnerGymRaw(int gymId, CancellationToken ct);

        Task<List<ResponsePartnerGymDTO>> GetPartnerGymByStateRaw(string state, CancellationToken ct);
        Task<List<ResponsePartnerGymDTO>> GetPartnerGymByCountryRaw(string country, CancellationToken ct);
        Task<List<ResponsePartnerGymDTO>> GetPartnerGymByCityRaw(string city, CancellationToken ct);
        Task<List<ResponsePartnerGymDTO>> GetPartnerGymByNameRaw(string name, CancellationToken ct);
    }
}
