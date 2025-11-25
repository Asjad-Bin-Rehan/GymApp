using BS.Services.UserService.DTOs;
using static BS.Services.UserService.UserService;

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

        //suspend
        //Task<bool> SuspendUserRaw(int userId, int adminId, CancellationToken ct);

        // Authentication
        Task<(ResponseUserDTO? user, string? errorMessage)> LoginUserRaw(LoginUserDTO request, CancellationToken ct);

        Task<GetUserCountDTO> GetUserCountRaw(CancellationToken ct);

        Task<SuspendUserResult> SuspendUserRaw(SuspendUserDTO request, CancellationToken ct);
        Task<ActivateUserResult> ActivateUserRaw(SuspendUserDTO request, CancellationToken ct);


    }
}
