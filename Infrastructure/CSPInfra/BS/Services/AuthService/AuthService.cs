using BS.Services.AuthService.DTOs;
using DA;
using DM.DomainModels;
using Helpers.Auth.Models;
using Helpers.CustomExceptionThrower;
using Helpers.StringsExtension;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BS.Services.AuthService
{
    public class AuthService : IAuthService
    {
        IUnitOfWork _uow;
        public AuthService(IUnitOfWork uow) 
        {  
            _uow = uow;
        }

        public async Task<bool> SignUp(RequestSignUp request, string DeviceId, CancellationToken ct)
        {
            var query = _uow.encrypted_credentials.GetQueryable();
            var existingUser = await query.Data.FirstOrDefaultAsync(x => (x.UserId==request.UserId || x.Email==request.Email) && x.IsActive);
            ArgumentFalseException.ThrowIfFalse(existingUser==null, "Email already exists");

            #region ToDomain
            var (hash, salt) = PasswordHelper.HashPassword(request.Password ?? throw new ArgumentFalseException("Password invalid for encryption"));
            var encryptedCredentials = new Encrypted_Credentials()
            {
                DeviceId = DeviceId,
                Name = request.Name,
                Email = request.Email,
                UserId = request.UserId,
                UserType = request.UserType,
                Token = request.Token,
                RefreshToken = request.RefreshToken,
                Hash = hash,
                Salt = salt,

                Id = Guid.NewGuid().ToString(),
                CreatedBy = DeviceId,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = DeviceId,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true,
                IsArchived = false,
            };
            #endregion ToDomain

            await _uow.encrypted_credentials.AddAsync(encryptedCredentials, DeviceId, ct);
            await _uow.CommitAsync(ct);
            return true;
        }

        public async Task<ResponseAuthorizedUser> Login(RequestLogin request, CancellationToken ct)
        {
            var (hash, salt) = PasswordHelper.HashPassword(request.Password ?? throw new ArgumentFalseException("Password invalid for decryption"));
            var query = _uow.encrypted_credentials.GetQueryable();
            var existingUser = await query.Data.FirstOrDefaultAsync(x => x.Email==request.Email);
            ArgumentFalseException.ThrowIfFalse(existingUser!=null, "Email does not exist");
            ArgumentFalseException.ThrowIfFalse(PasswordHelper.VerifyPassword(request.Password, existingUser?.Hash ?? string.Empty, existingUser?.Salt ?? string.Empty), "Incorrect Password");

            return new ResponseAuthorizedUser()
            {
                Name = existingUser?.Name ?? string.Empty,
                Email = existingUser?.Email ?? string.Empty,
                UserId = existingUser?.UserId ?? string.Empty,
                UserType = existingUser?.UserType ?? string.Empty,
                Token = existingUser?.Token ?? string.Empty,
                RefreshToken = existingUser?.RefreshToken ?? string.Empty,
            };
        }

        public async Task<AccessAndRefreshTokens> GetRefreshToken(AccessAndRefreshTokens request, CancellationToken ct)
        {
            return new AccessAndRefreshTokens()
            {
                AccessToken = request.AccessToken,
                RefreshToken = request.RefreshToken,
            };
        }
    }
}
