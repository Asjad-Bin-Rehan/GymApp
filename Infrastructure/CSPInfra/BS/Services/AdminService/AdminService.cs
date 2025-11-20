using BS.Services.AdminService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BS.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AdminService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        // ------------------- CREATE -------------------
        public async Task<int> SignUpAdmin(SignUpAdminDTO dto, CancellationToken ct)
        {
            await _dbContext.Database.OpenConnectionAsync(ct);
            await using var conn = _dbContext.Database.GetDbConnection();
            await using var cmd = conn.CreateCommand();

            // 1. Check username exists
            cmd.CommandText = "SELECT COUNT(*) FROM admins WHERE username = @username";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new NpgsqlParameter("@username", dto.username));

            var usernameExists = (long)await cmd.ExecuteScalarAsync(ct);
            if (usernameExists > 0)
                throw new Exception("USERNAME_EXISTS");

            // 2. Check email exists
            cmd.CommandText = "SELECT COUNT(*) FROM admins WHERE email = @email";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new NpgsqlParameter("@email", dto.email));

            var emailExists = (long)await cmd.ExecuteScalarAsync(ct);
            if (emailExists > 0)
                throw new Exception("EMAIL_EXISTS");

            // 3. Insert admin
            var passwordHash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(dto.password))
            );

            cmd.CommandText = @"
INSERT INTO admins (username, password_hash, role, full_name, phone, date_of_birth, email)
VALUES (@username, @password_hash, @role, @full_name, @phone, @date_of_birth, @email)
RETURNING admin_id;
";

            cmd.Parameters.Clear();
            cmd.Parameters.Add(new NpgsqlParameter("@username", dto.username));
            cmd.Parameters.Add(new NpgsqlParameter("@password_hash", passwordHash));
            cmd.Parameters.Add(new NpgsqlParameter("@role", dto.role));
            cmd.Parameters.Add(new NpgsqlParameter("@full_name", dto.full_name ?? "Unknown"));
            cmd.Parameters.Add(new NpgsqlParameter("@phone", (object?)dto.phone ?? DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@date_of_birth", (object?)dto.date_of_birth ?? DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@email", dto.email));

            var result = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt32(result);
        }


        // ------------------- READ -------------------
        public async Task<AdminDTO?> GetAdminById(int adminId, CancellationToken ct)
        {
            var sql = @"SELECT admin_id, username, role, full_name, phone, date_of_birth, join_date, email 
                        FROM admins 
                        WHERE admin_id = @admin_id";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@admin_id", adminId));

            await _dbContext.Database.OpenConnectionAsync(ct);
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            if (!reader.HasRows) return null;
            await reader.ReadAsync(ct);

            return new AdminDTO
            {
                admin_id = reader.GetInt32(0),
                username = reader.GetString(1),
                role = reader.GetString(2),
                full_name = reader.GetString(3),
                phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                date_of_birth = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                join_date = reader.GetDateTime(6),
                email = reader.GetString(7)
            };
        }

        public async Task<List<AdminDTO>> ListAllAdmins(CancellationToken ct)
        {
            var sql = @"SELECT admin_id, username, role, full_name, phone, date_of_birth, join_date, email 
                        FROM admins";
            var list = new List<AdminDTO>();

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            await _dbContext.Database.OpenConnectionAsync(ct);

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new AdminDTO
                {
                    admin_id = reader.GetInt32(0),
                    username = reader.GetString(1),
                    role = reader.GetString(2),
                    full_name = reader.GetString(3),
                    phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                    date_of_birth = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                    join_date = reader.GetDateTime(6),
                    email = reader.GetString(7)
                });
            }

            return list;
        }

        // ------------------- UPDATE -------------------
        public async Task<bool> UpdateAdmin(UpdateAdminDTO dto, CancellationToken ct)
        {
            var passwordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(dto.password)));

            var sql = @"
UPDATE admins
SET username = @username,
    password_hash = @password_hash,
    role = @role,
    full_name = @full_name,
    phone = @phone,
    date_of_birth = @date_of_birth,
    email = @email
WHERE admin_id = @admin_id;
";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@admin_id", dto.admin_id));
            cmd.Parameters.Add(new NpgsqlParameter("@username", dto.username ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@password_hash", passwordHash));
            cmd.Parameters.Add(new NpgsqlParameter("@role", dto.role ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@full_name", dto.full_name ?? "Unknown"));
            cmd.Parameters.Add(new NpgsqlParameter("@phone", dto.phone ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@date_of_birth", dto.date_of_birth ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@email", dto.email ?? (object)DBNull.Value));

            await _dbContext.Database.OpenConnectionAsync(ct);
            var result = await cmd.ExecuteNonQueryAsync(ct);
            return result > 0;
        }

        // ------------------- DELETE -------------------
        public async Task<bool> DeleteAdmin(int adminId, CancellationToken ct)
        {
            var sql = "DELETE FROM admins WHERE admin_id = @admin_id";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@admin_id", adminId));

            await _dbContext.Database.OpenConnectionAsync(ct);
            var result = await cmd.ExecuteNonQueryAsync(ct);
            return result > 0;
        }

        // ------------------- LOGIN (with JWT) -------------------
        public async Task<LoginAdminWithoutJWTResponseDTO> LoginAdminWithoutJWT(LoginAdminWithoutJWTDTO dto, CancellationToken ct)
        {
            var passwordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(dto.Password)));

            var sql = @"SELECT admin_id, username, role, full_name, phone, date_of_birth, join_date, email
                        FROM admins 
                        WHERE username = @username AND password_hash = @password_hash";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@username", dto.Username ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@password_hash", passwordHash));

            await _dbContext.Database.OpenConnectionAsync(ct);
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            if (!reader.HasRows)
            {
                return new LoginAdminWithoutJWTResponseDTO
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            await reader.ReadAsync(ct);
            
            var adminId = reader.GetInt32(0);
            var username = reader.GetString(1);
            
            // Generate JWT Token
            var token = GenerateJwtToken(adminId, username);

            Console.WriteLine("✅ ADMIN LOGIN SUCCESS:");
            Console.WriteLine($"   Admin ID: {adminId}");
            Console.WriteLine($"   Username: {username}");
            Console.WriteLine($"   Token Generated: {token[..50]}..."); // Show first 50 chars

            return new LoginAdminWithoutJWTResponseDTO
            {
                admin_id = adminId,
                username = username,
                role = reader.GetString(2),
                full_name = reader.GetString(3),
                phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                date_of_birth = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                join_date = reader.GetDateTime(6),
                email = reader.GetString(7),
                Success = true,
                Message = "Login successful",
                token = token
            };
        }

        private string GenerateJwtToken(int adminId, string username)
        {
            var secret = _configuration["Jwt:Secret"] ?? throw new Exception("JWT Secret not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, adminId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(ClaimTypes.Role, "Admin") // Add admin role claim
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
    }
}
