using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using BS.Services.UserService.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BS.Services.UserService
{
    public class UserService : IUserServiceRaw
    {
        private readonly GymverseDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserService(GymverseDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        // ============================================================
        // ADD USER (RAW SQL WITH PASSWORD HASHING)
        // ============================================================
        public async Task<bool> AddUserRaw(AddUserDTO request, CancellationToken ct)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var sqlQuery = @"
                INSERT INTO public.""Users""
                (
                    ""username"", ""password_hash"", ""full_name"", ""email"",
                    ""phone"", ""date_of_birth"", ""join_date"", ""membership_id"",
                    ""status"", ""total_points""
                )
                VALUES
                (
                    @Username, @PasswordHash, @FullName, @Email,
                    @Phone, @DateOfBirth, @JoinDate, @MembershipId,
                    @Status, @TotalPoints
                );
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@Username", request.Username ?? (object)DBNull.Value),
                new NpgsqlParameter("@PasswordHash", passwordHash),
                new NpgsqlParameter("@FullName", request.FullName ?? (object)DBNull.Value),
                new NpgsqlParameter("@Email", request.Email ?? (object)DBNull.Value),
                new NpgsqlParameter("@Phone", request.Phone ?? (object)DBNull.Value),
                new NpgsqlParameter("@DateOfBirth", request.DateOfBirth ?? (object)DBNull.Value),
                new NpgsqlParameter("@JoinDate", DateTime.UtcNow),
                new NpgsqlParameter("@MembershipId", request.MembershipId ?? (object)DBNull.Value),
                new NpgsqlParameter("@Status", request.Status ?? "Active"),
                new NpgsqlParameter("@TotalPoints", request.TotalPoints)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, parameters, ct);
            return true;
        }

        // ============================================================
        // LOGIN USER
        // ============================================================
        public async Task<ResponseUserDTO?> LoginUserRaw(LoginUserDTO request, CancellationToken ct)
        {
            var sqlQuery = @"
                SELECT 
                    ""user_id"", ""username"", ""password_hash"", ""full_name"", ""email"", 
                    ""phone"", ""status"", ""membership_id"", ""total_points""
                FROM public.""Users""
                WHERE ""username"" = @UsernameOrEmail OR ""email"" = @UsernameOrEmail;
            ";

            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sqlQuery;
            cmd.Parameters.Add(new NpgsqlParameter("@UsernameOrEmail", request.UsernameOrEmail));

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct)) return null;

            var passwordHash = reader.GetString(reader.GetOrdinal("password_hash"));
            if (!BCrypt.Net.BCrypt.Verify(request.Password, passwordHash)) return null;

            var user = new ResponseUserDTO
            {
                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                Username = reader.GetString(reader.GetOrdinal("username")),
                FullName = reader.GetString(reader.GetOrdinal("full_name")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                Status = reader.GetString(reader.GetOrdinal("status")),
                MembershipId = reader.IsDBNull(reader.GetOrdinal("membership_id")) ? null : reader.GetString(reader.GetOrdinal("membership_id")),
                TotalPoints = reader.GetInt32(reader.GetOrdinal("total_points"))
            };

            // Generate JWT
            //user.Token = GenerateJwtToken(user.UserId, user.Username);

            return user;
        }

        private string GenerateJwtToken(int userId, string username)
        {
            var secret = _configuration["Jwt:Secret"] ?? throw new Exception("JWT Secret not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ============================================================
        // GET USER BY ID
        // ============================================================
        public async Task<ResponseUserDTO?> GetUserByIdRaw(int userId, CancellationToken ct)
        {
            var sqlQuery = @"
                SELECT 
                    ""user_id"" AS ""UserId"",
                    ""username"" AS ""Username"",
                    ""full_name"" AS ""FullName"",
                    ""email"" AS ""Email"",
                    ""phone"" AS ""Phone"",
                    ""date_of_birth"" AS ""DateOfBirth"",
                    ""join_date"" AS ""JoinDate"",
                    ""membership_id"" AS ""MembershipId"",
                    ""status"" AS ""Status"",
                    ""total_points"" AS ""TotalPoints""
                FROM public.""Users""
                WHERE ""user_id"" = @UserId;
            ";

            var parameter = new NpgsqlParameter("@UserId", userId);

            return await _dbContext.ResponseUserDTOs
                .FromSqlRaw(sqlQuery, parameter)
                .FirstOrDefaultAsync(ct);
        }

        // ============================================================
        // LIST USERS
        // ============================================================
        public async Task<List<ResponseUserDTO>> ListAllUsersRaw(int limit, int offset, CancellationToken ct)
        {
            var sqlQuery = @"
                SELECT 
                    ""user_id"" AS ""UserId"",
                    ""username"" AS ""Username"",
                    ""full_name"" AS ""FullName"",
                    ""email"" AS ""Email"",
                    ""phone"" AS ""Phone"",
                    ""date_of_birth"" AS ""DateOfBirth"",
                    ""join_date"" AS ""JoinDate"",
                    ""membership_id"" AS ""MembershipId"",
                    ""status"" AS ""Status"",
                    ""total_points"" AS ""TotalPoints""
                FROM public.""Users""
                ORDER BY ""user_id""
                LIMIT @Limit OFFSET @Offset;
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@Limit", limit),
                new NpgsqlParameter("@Offset", offset)
            };

            return await _dbContext.ResponseUserDTOs
                .FromSqlRaw(sqlQuery, parameters)
                .ToListAsync(ct);
        }

        // ============================================================
        // UPDATE USER
        // ============================================================
        public async Task<bool> UpdateUserRaw(UpdateUserDTO request, CancellationToken ct)
        {
            var sqlQuery = @"
                UPDATE public.""Users""
                SET
                    ""full_name"" = COALESCE(@FullName, ""full_name""),
                    ""email"" = COALESCE(@Email, ""email""),
                    ""phone"" = COALESCE(@Phone, ""phone""),
                    ""status"" = COALESCE(@Status, ""status""),
                    ""membership_id"" = COALESCE(@MembershipId, ""membership_id"")
                WHERE ""user_id"" = @UserId;
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@UserId", request.UserId),
                new NpgsqlParameter("@FullName", request.FullName ?? (object)DBNull.Value),
                new NpgsqlParameter("@Email", request.Email ?? (object)DBNull.Value),
                new NpgsqlParameter("@Phone", request.Phone ?? (object)DBNull.Value),
                new NpgsqlParameter("@Status", request.Status ?? (object)DBNull.Value),
                new NpgsqlParameter("@MembershipId", request.MembershipId ?? (object)DBNull.Value),
            };

            await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, parameters, ct);
            return true;
        }

        // ============================================================
        // DELETE USER
        // ============================================================
        public async Task<bool> DeleteUserRaw(int userId, CancellationToken ct)
        {
            var sqlQuery = @"
                DELETE FROM public.""Users""
                WHERE ""user_id"" = @UserId;
            ";

            var parameter = new NpgsqlParameter("@UserId", userId);
            await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, parameter, ct);
            return true;
        }

        // ============================================================
        // VALIDATIONS
        // ============================================================
        public async Task<bool> IsUsernameExistsRaw(string username, CancellationToken ct)
        {
            var sqlQuery = @"
                SELECT COUNT(*) 
                FROM public.""Users""
                WHERE ""username"" = @Username;
            ";

            var parameter = new NpgsqlParameter("@Username", username);
            var result = await _dbContext.Database.ExecuteScalarAsync(sqlQuery, parameter, ct);
            return Convert.ToInt32(result) > 0;
        }

        public async Task<bool> IsEmailExistsRaw(string email, CancellationToken ct)
        {
            var sqlQuery = @"
                SELECT COUNT(*) 
                FROM public.""Users""
                WHERE ""email"" = @Email;
            ";

            var parameter = new NpgsqlParameter("@Email", email);
            var result = await _dbContext.Database.ExecuteScalarAsync(sqlQuery, parameter, ct);
            return Convert.ToInt32(result) > 0;
        }
    }
}
