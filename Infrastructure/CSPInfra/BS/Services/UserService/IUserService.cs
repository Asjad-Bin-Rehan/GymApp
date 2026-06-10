using BS.Services.UserService.DTOs;
using Helpers.CommonModels;

namespace BS.Services.UserService
{
    public interface IUserService
    {
        Task<UpsertUserResponse> UpsertUser(UpsertUserRequest req, string userId, CancellationToken ct);
        Task<PagedResponse<GetUserResponse>> GetUser(string? userId, string? organizationId, string? email, string? name, string? userType, int lastCount, int skipRecords, CancellationToken ct);
    }
}
