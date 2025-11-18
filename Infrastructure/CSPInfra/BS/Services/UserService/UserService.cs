using BS.Services.UserService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BS.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        // ============================================================
        // ADD USER (RAW SQL WITH PASSWORD HASHING)
        // ============================================================
        public async Task<bool> AddUserRaw(SignupUserDTO request, CancellationToken ct)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.password);

            // Generate membership ID (e.g., MBR-20251118-XYZ12)
            string membershipId = $"MBR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6]}";

            var sqlQuery = @"
        INSERT INTO public.users
        (
            username, password_hash, full_name, email,
            phone, date_of_birth, join_date,
            membership_id, status, total_points
        )
        VALUES
        (
            @username, @passwordHash, @full_name, @email,
            @phone, @date_of_birth, @join_date,
            @membership_id, @status, @total_points
        );
    ";

            var parameters = new[]
            {
        new NpgsqlParameter("@username", request.username ?? (object)DBNull.Value),
        new NpgsqlParameter("@passwordHash", passwordHash),
        new NpgsqlParameter("@full_name", request.full_name ?? (object)DBNull.Value),
        new NpgsqlParameter("@email", request.email ?? (object)DBNull.Value),
        new NpgsqlParameter("@phone", request.phone ?? (object)DBNull.Value),
        new NpgsqlParameter("@date_of_birth", request.date_of_birth ?? (object)DBNull.Value),

        new NpgsqlParameter("@join_date", DateTime.UtcNow),

        // Auto membership ID
        new NpgsqlParameter("@membership_id", membershipId),

        // Always default Active
        new NpgsqlParameter("@status", "Active"),

        // Always default 0
        new NpgsqlParameter("@total_points", NpgsqlTypes.NpgsqlDbType.Integer) { Value = 0 }
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
                    user_id, username, password_hash, full_name, email,
                    phone, status, membership_id, total_points,
                    date_of_birth, join_date
                FROM public.users
                WHERE username = @username_or_email OR email = @username_or_email;
            ";

            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sqlQuery;
            cmd.Parameters.Add(new NpgsqlParameter("@username_or_email", request.username_or_email));

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct)) return null;

            var passwordHash = reader.GetString(reader.GetOrdinal("password_hash"));
            if (!BCrypt.Net.BCrypt.Verify(request.password, passwordHash)) return null;

            return new ResponseUserDTO
            {
                user_id = reader.GetInt32(reader.GetOrdinal("user_id")),
                username = reader.GetString(reader.GetOrdinal("username")),
                full_name = reader.GetString(reader.GetOrdinal("full_name")),
                email = reader.GetString(reader.GetOrdinal("email")),
                phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                status = reader.GetString(reader.GetOrdinal("status")),
                membership_id = reader.IsDBNull(reader.GetOrdinal("membership_id")) ? null : reader.GetString(reader.GetOrdinal("membership_id")),
                total_points = reader.GetInt32(reader.GetOrdinal("total_points")),
                date_of_birth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? null : reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                join_date = reader.GetDateTime(reader.GetOrdinal("join_date"))
            };
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
                    user_id, username, full_name, email,
                    phone, date_of_birth, join_date,
                    membership_id, status, total_points
                FROM public.users
                WHERE user_id = @user_id
            ";

            var parameter = new NpgsqlParameter("@user_id", userId);

            var result = await _dbContext
                                .Database
                                .SqlQueryRaw<ResponseUserDTO>(sqlQuery, parameter)
                                .FirstOrDefaultAsync(ct);

            return result;
        }


        public async Task<ResponseUserDTO?> GetUserByIdRawWithManualMapping(int userId, CancellationToken ct)
        {
            var sqlQuery = @"
        SELECT 
            user_id, username, full_name, email,
            phone, date_of_birth, join_date,
            membership_id, status, total_points
        FROM public.users
        WHERE user_id = @user_id;
    ";

            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sqlQuery;
            cmd.Parameters.Add(new NpgsqlParameter("@user_id", userId));

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct)) return null;

            return new ResponseUserDTO
            {
                user_id = reader.GetInt32(reader.GetOrdinal("user_id")),
                username = reader.GetString(reader.GetOrdinal("username")),
                full_name = reader.GetString(reader.GetOrdinal("full_name")),
                email = reader.GetString(reader.GetOrdinal("email")),
                phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                date_of_birth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? null : reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                join_date = reader.GetDateTime(reader.GetOrdinal("join_date")),
                membership_id = reader.IsDBNull(reader.GetOrdinal("membership_id")) ? null : reader.GetString(reader.GetOrdinal("membership_id")),
                status = reader.GetString(reader.GetOrdinal("status")),
                total_points = reader.GetInt32(reader.GetOrdinal("total_points"))
            };
        }


        // ============================================================
        // LIST USERS
        // ============================================================
        public async Task<List<ResponseUserDTO>> ListAllUsersRaw(int limit, int offset, CancellationToken ct)
        {
            var sqlQuery = @"
                SELECT 
                    user_id, username, full_name, email,
                    phone, date_of_birth, join_date,
                    membership_id, status, total_points
                FROM public.users
                ORDER BY user_id
                LIMIT @Limit OFFSET @Offset;
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@Limit", limit),
                new NpgsqlParameter("@Offset", offset)
            };

            var users = await _dbContext
                            .Database
                            .SqlQueryRaw<ResponseUserDTO>(sqlQuery, parameters)
                            .ToListAsync(ct);

            return users;
        }

        // ============================================================
        // UPDATE USER
        // ============================================================
        public async Task<bool> UpdateUserRaw(UpdateUserDTO request, CancellationToken ct)
        {
            var sqlQuery = @"
                UPDATE public.users
                SET
                    full_name = COALESCE(@full_name, full_name),
                    email = COALESCE(@email, email),
                    phone = COALESCE(@phone, phone),
                    status = COALESCE(@status, status),
                    membership_id = COALESCE(@membership_id, membership_id)
                WHERE user_id = @user_id;
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@user_id", request.user_id),
                new NpgsqlParameter("@full_name", request.full_name ?? (object)DBNull.Value),
                new NpgsqlParameter("@email", request.email ?? (object)DBNull.Value),
                new NpgsqlParameter("@phone", request.phone ?? (object)DBNull.Value),
                new NpgsqlParameter("@status", request.status ?? (object)DBNull.Value),
                new NpgsqlParameter("@membership_id", request.membership_id ?? (object)DBNull.Value)
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
        DELETE FROM public.users
        WHERE user_id = @user_id
        ";

            var parameter = new NpgsqlParameter("@user_id", userId);

            await _dbContext.Database.ExecuteSqlRawAsync(
                sqlQuery,
                new object[] { parameter },
                ct
            );

            return true;
        }

        // ============================================================
        // VALIDATIONS
        // ============================================================
        public async Task<bool> IsUsernameExistsRaw(string username, CancellationToken ct)
        {
            var sqlQuery = @"
                SELECT COUNT(*) AS count
                FROM public.users
                WHERE username = @username
            ";

            var parameter = new NpgsqlParameter("@username", username);

            var result = await _dbContext
                                .Database
                                .SqlQueryRaw<IntScalar>(sqlQuery, parameter)
                                .FirstOrDefaultAsync(ct);

            return result?.count > 0;
        }

        public async Task<bool> IsEmailExistsRaw(string email, CancellationToken ct)
        {
            var sqlQuery = @"
                SELECT COUNT(*) AS count
                FROM public.users
                WHERE email = @email
            ";

            var parameter = new NpgsqlParameter("@email", email);

            var result = await _dbContext
                                .Database
                                .SqlQueryRaw<IntScalar>(sqlQuery, parameter)
                                .FirstOrDefaultAsync(ct);

            return result?.count > 0;
        }

        // Helper class for COUNT(*) scalar queries
        public class IntScalar
        {
            public int count { get; set; }
        }
    }
}
