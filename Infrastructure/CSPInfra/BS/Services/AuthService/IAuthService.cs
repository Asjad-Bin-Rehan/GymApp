using BS.Services.AuthService.DTOs;
using Helpers.Auth.Models;

namespace BS.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> SignUp(RequestSignUp request, string DeviceId, CancellationToken ct);
        Task<ResponseAuthorizedUser> Login(RequestLogin request, CancellationToken ct);
        Task<AccessAndRefreshTokens> GetRefreshToken(AccessAndRefreshTokens request, CancellationToken ct);

    }
}
