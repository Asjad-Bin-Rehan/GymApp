using BS.Services.UserService.DTOs;

namespace BS.Services.UserService
{
    public interface IUserService
    {
        // Create (RAW)
        Task<bool> AddUserRaw(SignupUserDTO request, CancellationToken ct);

        // Read / List / Get (RAW)
        Task<ResponseUserDTO?> GetUserByIdRaw(int userId, CancellationToken ct);

        Task<ResponseUserDTO?> GetUserByIdRawWithManualMapping(int userId, CancellationToken ct);
        Task<List<ResponseUserDTO>> ListAllUsersRaw(int limit, int offset, CancellationToken ct);

        // Update (RAW)
        Task<bool> UpdateUserRaw(UpdateUserDTO request, CancellationToken ct);

        // Delete (RAW)
        Task<bool> DeleteUserRaw(int userId, CancellationToken ct);

        // Validations (RAW)
        Task<bool> IsUsernameExistsRaw(string username, CancellationToken ct);
        Task<bool> IsEmailExistsRaw(string email, CancellationToken ct);

        // Authentication
        Task<(ResponseUserDTO? user, string? errorMessage)> LoginUserRaw(LoginUserDTO request, CancellationToken ct);

    }
}
