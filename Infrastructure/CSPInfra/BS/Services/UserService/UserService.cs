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

    // Generate membership ID
    string membershipId = $"MBR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6]}";

    await using var conn = _dbContext.Database.GetDbConnection();
    await conn.OpenAsync(ct);

    // Use transaction so signup + subscription is atomic
    await using var transaction = await conn.BeginTransactionAsync(ct);

    try
    {
        // ---------------------------
        // 1. INSERT USER
        // ---------------------------
        var insertUserSql = @"
            INSERT INTO Users
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
            )
            RETURNING user_id;
        ";

        await using var insertUserCmd = conn.CreateCommand();
        insertUserCmd.Transaction = transaction;
        insertUserCmd.CommandText = insertUserSql;

        insertUserCmd.Parameters.Add(new NpgsqlParameter("@username", request.username ?? (object)DBNull.Value));
        insertUserCmd.Parameters.Add(new NpgsqlParameter("@passwordHash", passwordHash));
        insertUserCmd.Parameters.Add(new NpgsqlParameter("@full_name", request.full_name ?? (object)DBNull.Value));
        insertUserCmd.Parameters.Add(new NpgsqlParameter("@email", request.email ?? (object)DBNull.Value));
        insertUserCmd.Parameters.Add(new NpgsqlParameter("@phone", request.phone ?? (object)DBNull.Value));
        insertUserCmd.Parameters.Add(new NpgsqlParameter("@date_of_birth", request.date_of_birth ?? (object)DBNull.Value));
        insertUserCmd.Parameters.Add(new NpgsqlParameter("@join_date", DateTime.UtcNow));
        insertUserCmd.Parameters.Add(new NpgsqlParameter("@membership_id", membershipId));
        insertUserCmd.Parameters.Add(new NpgsqlParameter("@status", "Active"));
        insertUserCmd.Parameters.Add(new NpgsqlParameter("@total_points", NpgsqlTypes.NpgsqlDbType.Integer)
{
    Value = 0
});


        // Get inserted user_id
        var userId = (int)(await insertUserCmd.ExecuteScalarAsync(ct));

        // ---------------------------
        // 2. GET PLAN DURATION
        // ---------------------------
        var getPlanSql = @"
            SELECT duration_months 
            FROM MembershipPlans 
            WHERE plan_id = @plan_id;
        ";

        await using var getPlanCmd = conn.CreateCommand();
        getPlanCmd.Transaction = transaction;
        getPlanCmd.CommandText = getPlanSql;
        getPlanCmd.Parameters.Add(new NpgsqlParameter("@plan_id", request.plan_id));

        var durationObj = await getPlanCmd.ExecuteScalarAsync(ct);

        if (durationObj == null)
            throw new Exception("Invalid plan_id selected.");

        int durationMonths = Convert.ToInt32(durationObj);

        DateTime startDate = DateTime.UtcNow.Date;
        DateTime endDate = startDate.AddMonths(durationMonths);

        // ---------------------------
        // 3. INSERT SUBSCRIPTION
        // ---------------------------
        var insertSubscriptionSql = @"
            INSERT INTO Subscriptions (user_id, plan_id, start_date, end_date, payment_status)
            VALUES (@user_id, @plan_id, @start_date, @end_date, 'Paid');
        ";

        await using var insertSubCmd = conn.CreateCommand();
        insertSubCmd.Transaction = transaction;
        insertSubCmd.CommandText = insertSubscriptionSql;

        insertSubCmd.Parameters.Add(new NpgsqlParameter("@user_id", userId));
        insertSubCmd.Parameters.Add(new NpgsqlParameter("@plan_id", request.plan_id));
        insertSubCmd.Parameters.Add(new NpgsqlParameter("@start_date", startDate));
        insertSubCmd.Parameters.Add(new NpgsqlParameter("@end_date", endDate));

        await insertSubCmd.ExecuteNonQueryAsync(ct);

        // Commit both inserts
        await transaction.CommitAsync(ct);

        return true;
    }
    catch
    {
        await transaction.RollbackAsync(ct);
        throw;
    }
}




        // ============================================================
        // LOGIN USER
        // ============================================================
        public async Task<(ResponseUserDTO? user, string? errorMessage)> LoginUserRaw(LoginUserDTO request, CancellationToken ct)
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
            if (!await reader.ReadAsync(ct))
            {
                return (null, "User not found");
            }

            var passwordHash = reader.GetString(reader.GetOrdinal("password_hash"));
            if (!BCrypt.Net.BCrypt.Verify(request.password, passwordHash))
            {
                return (null, "Invalid password");
            }

            var status = reader.GetString(reader.GetOrdinal("status"));
            if (status != "Active")
            {
                return (null, status == "Inactive"
                    ? "Your account is inactive. Please contact support."
                    : "Your account has expired. Please renew your membership.");
            }

            var userId = reader.GetInt32(reader.GetOrdinal("user_id"));
            var username = reader.GetString(reader.GetOrdinal("username"));

            // Generate JWT Token
            var token = GenerateJwtToken(userId, username);

            var userDto = new ResponseUserDTO
            {
                user_id = userId,
                username = username,
                full_name = reader.GetString(reader.GetOrdinal("full_name")),
                email = reader.GetString(reader.GetOrdinal("email")),
                phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                status = status,
                membership_id = reader.IsDBNull(reader.GetOrdinal("membership_id")) ? null : reader.GetString(reader.GetOrdinal("membership_id")),
                total_points = reader.GetInt32(reader.GetOrdinal("total_points")),
                date_of_birth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? null : reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                join_date = reader.GetDateTime(reader.GetOrdinal("join_date")),
                token = token
            };

            return (userDto, null);
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
                total_points = reader.GetInt32(reader.GetOrdinal("total_points")),
                token = null // Not populated when fetching by ID
            };
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

            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sqlQuery;
            cmd.Parameters.Add(new NpgsqlParameter("@Limit", limit));
            cmd.Parameters.Add(new NpgsqlParameter("@Offset", offset));

            var users = new List<ResponseUserDTO>();

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                users.Add(new ResponseUserDTO
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
                    total_points = reader.GetInt32(reader.GetOrdinal("total_points")),
                    token = null
                });
            }

            return users;
        }


        // ============================================================
        // SUSPEND USER (ONLY SUPERADMIN)
        // ============================================================
        public async Task<bool> SuspendUserRaw(int userId, int adminId, CancellationToken ct)
        {
            if (userId <= 0) throw new Exception("INVALID_USER_ID");
            if (adminId <= 0) throw new Exception("INVALID_ADMIN_ID");

            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            // 1. Check if admin exists and is SuperAdmin
            await using var adminCmd = conn.CreateCommand();
            adminCmd.CommandText = @"
        SELECT role
        FROM public.admins
        WHERE admin_id = @AdminId;
    ";
            adminCmd.Parameters.Add(new NpgsqlParameter("@AdminId", adminId));

            var roleObj = await adminCmd.ExecuteScalarAsync(ct);
            if (roleObj == null) throw new Exception("ADMIN_NOT_FOUND");

            var role = roleObj.ToString();
            if (role != "SuperAdmin") throw new Exception("UNAUTHORIZED");

            // 2. Check if user exists
            await using var userCmd = conn.CreateCommand();
            userCmd.CommandText = @"
        SELECT status
        FROM public.users
        WHERE user_id = @UserId;
    ";
            userCmd.Parameters.Add(new NpgsqlParameter("@UserId", userId));

            var statusObj = await userCmd.ExecuteScalarAsync(ct);
            if (statusObj == null) throw new Exception("USER_NOT_FOUND");

            // 3. Update user's status to 'Inactive'
            await using var updateCmd = conn.CreateCommand();
            updateCmd.CommandText = @"
        UPDATE public.users
        SET status = 'Inactive'
        WHERE user_id = @UserId;
    ";
            updateCmd.Parameters.Add(new NpgsqlParameter("@UserId", userId));

            var rowsAffected = await updateCmd.ExecuteNonQueryAsync(ct);

            return rowsAffected > 0;
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

        public async Task<GetUserCountDTO> GetUserCountRaw(CancellationToken ct)
        {
            var sqlQuery = @"
        SELECT COUNT(*) AS total_users
        FROM public.users;
    ";

            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sqlQuery;

            var resultObj = await cmd.ExecuteScalarAsync(ct);

            return new GetUserCountDTO
            {
                total_users = Convert.ToInt32(resultObj)
            };
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
