using BS.Services.AdminService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Security.Cryptography;
using System.Text;

namespace BS.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _dbContext;

        public AdminService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ------------------- CREATE -------------------
        public async Task<int> SignUpAdmin(SignUpAdminDTO dto, CancellationToken ct)
        {
            var passwordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(dto.password)));

            var sql = @"
INSERT INTO admins (username, password_hash, role)
VALUES (@username, @password_hash, @role)
RETURNING admin_id;
";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@username", dto.username ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@password_hash", passwordHash));
            cmd.Parameters.Add(new NpgsqlParameter("@role", dto.role ?? (object)DBNull.Value));

            await _dbContext.Database.OpenConnectionAsync(ct);
            var result = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt32(result);
        }

        // ------------------- READ -------------------
        public async Task<AdminDTO?> GetAdminById(int adminId, CancellationToken ct)
        {
            var sql = "SELECT admin_id, username, role FROM admins WHERE admin_id = @admin_id";

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
                role = reader.GetString(2)
            };
        }

        public async Task<List<AdminDTO>> ListAllAdmins(CancellationToken ct)
        {
            var sql = "SELECT admin_id, username, role FROM admins";
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
                    role = reader.GetString(2)
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
    role = @role
WHERE admin_id = @admin_id;
";

            await using var cmd = _dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new NpgsqlParameter("@admin_id", dto.admin_id));
            cmd.Parameters.Add(new NpgsqlParameter("@username", dto.username ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@password_hash", passwordHash));
            cmd.Parameters.Add(new NpgsqlParameter("@role", dto.role ?? (object)DBNull.Value));

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

        // ------------------- LOGIN (without JWT) -------------------
        public async Task<LoginAdminWithoutJWTResponseDTO> LoginAdminWithoutJWT(LoginAdminWithoutJWTDTO dto, CancellationToken ct)
        {
            var passwordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(dto.Password)));

            var sql = "SELECT admin_id, username, role FROM admins WHERE username = @username AND password_hash = @password_hash";

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
            return new LoginAdminWithoutJWTResponseDTO
            {
                AdminId = reader.GetInt32(0),
                Username = reader.GetString(1),
                Role = reader.GetString(2),
                Success = true,
                Message = "Login successful"
            };
        }
    }
}
