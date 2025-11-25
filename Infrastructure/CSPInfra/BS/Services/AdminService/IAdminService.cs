using BS.Services.AdminService.DTOs;

namespace BS.Services.AdminService
{
    public interface IAdminService
    {
        // Create
        Task<int> SignUpAdmin(SignUpAdminDTO request, CancellationToken ct);

        // Read
        Task<AdminDTO?> GetAdminById(int adminId, CancellationToken ct);
        Task<List<AdminDTO>> ListAllAdmins(CancellationToken ct);

        // Update
        Task<bool> UpdateAdmin(UpdateAdminDTO request, CancellationToken ct);

        // Delete
        Task<bool> DeleteAdmin(int adminId, CancellationToken ct);

        // Login without JWT
        Task<LoginAdminWithoutJWTResponseDTO> LoginAdminWithoutJWT(LoginAdminWithoutJWTDTO request, CancellationToken ct);

        Task<ResponseTotalRevenueDTO> GetTotalRevenueRaw(CancellationToken ct);
    }
}
