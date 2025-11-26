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
                    ? "Your account is inactive/suspended. Please contact support: info@gymverse.com"
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


        public async Task<List<ViewUserMembershipDTO>> ListUsersWithMembershipRaw(int limit, int offset, CancellationToken ct)
        {
            var sqlQuery = @"
        SELECT 
            user_id, username, full_name, email,
            phone, date_of_birth, join_date, status, total_points,
            subscription_id, plan_id, membership_start_date, 
            membership_end_date, payment_status,
            plan_name, duration_months, price
        FROM public.view_user_with_membership
        ORDER BY user_id
        LIMIT @Limit OFFSET @Offset;
    ";

            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sqlQuery;
            cmd.Parameters.Add(new NpgsqlParameter("@Limit", limit));
            cmd.Parameters.Add(new NpgsqlParameter("@Offset", offset));

            var users = new List<ViewUserMembershipDTO>();

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                users.Add(new ViewUserMembershipDTO
                {
                    user_id = reader.GetInt32(reader.GetOrdinal("user_id")),
                    username = reader.GetString(reader.GetOrdinal("username")),
                    full_name = reader.GetString(reader.GetOrdinal("full_name")),
                    email = reader.GetString(reader.GetOrdinal("email")),
                    phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                    date_of_birth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? null : reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                    join_date = reader.GetDateTime(reader.GetOrdinal("join_date")),
                    status = reader.GetString(reader.GetOrdinal("status")),
                    total_points = reader.GetInt32(reader.GetOrdinal("total_points")),

                    subscription_id = reader.IsDBNull(reader.GetOrdinal("subscription_id")) ? null : reader.GetInt32(reader.GetOrdinal("subscription_id")),
                    plan_id = reader.IsDBNull(reader.GetOrdinal("plan_id")) ? null : reader.GetInt32(reader.GetOrdinal("plan_id")),
                    membership_start_date = reader.IsDBNull(reader.GetOrdinal("membership_start_date")) ? null : reader.GetDateTime(reader.GetOrdinal("membership_start_date")),
                    membership_end_date = reader.IsDBNull(reader.GetOrdinal("membership_end_date")) ? null : reader.GetDateTime(reader.GetOrdinal("membership_end_date")),
                    payment_status = reader.IsDBNull(reader.GetOrdinal("payment_status")) ? null : reader.GetString(reader.GetOrdinal("payment_status")),

                    plan_name = reader.IsDBNull(reader.GetOrdinal("plan_name")) ? null : reader.GetString(reader.GetOrdinal("plan_name")),
                    duration_months = reader.IsDBNull(reader.GetOrdinal("duration_months")) ? null : reader.GetInt32(reader.GetOrdinal("duration_months")),
                    price = reader.IsDBNull(reader.GetOrdinal("price")) ? null : reader.GetDecimal(reader.GetOrdinal("price")),
                });
            }

            return users;
        }


        public enum SuspendUserResult
        {
            Success,
            InvalidRequest,
            InvalidUserId,
            InvalidAdminId,
            AdminNotFound,
            Unauthorized,
            UserNotFound,
            Failed
        }


        public async Task<SuspendUserResult> SuspendUserRaw(SuspendUserDTO request, CancellationToken ct)
        {
            if (request == null) return SuspendUserResult.InvalidRequest;
            if (request.user_id <= 0) return SuspendUserResult.InvalidUserId;
            if (request.admin_id <= 0) return SuspendUserResult.InvalidAdminId;

            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            await using var tx = await conn.BeginTransactionAsync(ct);

            try
            {
                // 1️⃣ CHECK ADMIN ROLE
                await using (var adminCmd = conn.CreateCommand())
                {
                    adminCmd.Transaction = tx;
                    adminCmd.CommandText = @"SELECT role FROM public.admins WHERE admin_id = @AdminId";
                    adminCmd.Parameters.Add(new NpgsqlParameter("@AdminId", request.admin_id));

                    var roleObj = await adminCmd.ExecuteScalarAsync(ct);
                    if (roleObj == null) return SuspendUserResult.AdminNotFound;
                    if (roleObj.ToString() != "SuperAdmin") return SuspendUserResult.Unauthorized;
                }

                // 2️⃣ CHECK USER EXISTS + GET POINTS
                int currentPoints;
                await using (var userCmd = conn.CreateCommand())
                {
                    userCmd.Transaction = tx;
                    userCmd.CommandText = @"SELECT total_points FROM public.users WHERE user_id = @UserId";
                    userCmd.Parameters.Add(new NpgsqlParameter("@UserId", request.user_id));

                    var ptObj = await userCmd.ExecuteScalarAsync(ct);
                    if (ptObj == null) return SuspendUserResult.UserNotFound;

                    currentPoints = Convert.ToInt32(ptObj);
                }

                // 3️⃣ UPDATE STATUS
                await using (var updateStatusCmd = conn.CreateCommand())
                {
                    updateStatusCmd.Transaction = tx;
                    updateStatusCmd.CommandText = @"
                UPDATE public.users SET status = 'Inactive'
                WHERE user_id = @UserId;
            ";
                    updateStatusCmd.Parameters.Add(new NpgsqlParameter("@UserId", request.user_id));
                    await updateStatusCmd.ExecuteNonQueryAsync(ct);
                }

                // 4️⃣ DEDUCT POINTS
                int newPoints = Math.Max(0, currentPoints - 100);
                await using (var pointsCmd = conn.CreateCommand())
                {
                    pointsCmd.Transaction = tx;
                    pointsCmd.CommandText = @"
                UPDATE public.users
                SET total_points = @NewPoints
                WHERE user_id = @UserId;
            ";
                    pointsCmd.Parameters.Add(new NpgsqlParameter("@NewPoints", newPoints));
                    pointsCmd.Parameters.Add(new NpgsqlParameter("@UserId", request.user_id));
                    await pointsCmd.ExecuteNonQueryAsync(ct);
                }

                // 5️⃣ INSERT HISTORY
                await using (var historyCmd = conn.CreateCommand())
                {
                    historyCmd.Transaction = tx;
                    historyCmd.CommandText = @"
                INSERT INTO public.pointshistory (user_id, points_change, reason)
                VALUES (@UserId, @Points, @Reason);
            ";
                    historyCmd.Parameters.Add(new NpgsqlParameter("@UserId", request.user_id));
                    historyCmd.Parameters.Add(new NpgsqlParameter("@Points", -100));
                    historyCmd.Parameters.Add(new NpgsqlParameter("@Reason", "User suspended by SuperAdmin"));

                    await historyCmd.ExecuteNonQueryAsync(ct);
                }

                await tx.CommitAsync(ct);
                return SuspendUserResult.Success;
            }
            catch
            {
                await tx.RollbackAsync(ct);
                return SuspendUserResult.Failed;
            }
        }

        public enum ActivateUserResult
        {
            Success,
            InvalidRequest,
            InvalidUserId,
            InvalidAdminId,
            AdminNotFound,
            Unauthorized,
            UserNotFound,
            Failed
        }


        public async Task<ActivateUserResult> ActivateUserRaw(SuspendUserDTO request, CancellationToken ct)
        {
            if (request == null) return ActivateUserResult.InvalidRequest;
            if (request.user_id <= 0) return ActivateUserResult.InvalidUserId;
            if (request.admin_id <= 0) return ActivateUserResult.InvalidAdminId;

            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            await using var tx = await conn.BeginTransactionAsync(ct);

            try
            {
                // 1️⃣ CHECK ADMIN ROLE
                await using (var adminCmd = conn.CreateCommand())
                {
                    adminCmd.Transaction = tx;
                    adminCmd.CommandText = @"SELECT role FROM public.admins WHERE admin_id = @AdminId";
                    adminCmd.Parameters.Add(new NpgsqlParameter("@AdminId", request.admin_id));

                    var roleObj = await adminCmd.ExecuteScalarAsync(ct);
                    if (roleObj == null) return ActivateUserResult.AdminNotFound;
                    if (roleObj.ToString() != "SuperAdmin") return ActivateUserResult.Unauthorized;
                }

                // 2️⃣ CHECK USER EXISTS + GET CURRENT POINTS
                int currentPoints;
                await using (var userCmd = conn.CreateCommand())
                {
                    userCmd.Transaction = tx;
                    userCmd.CommandText = @"SELECT total_points FROM public.users WHERE user_id = @UserId";
                    userCmd.Parameters.Add(new NpgsqlParameter("@UserId", request.user_id));

                    var ptObj = await userCmd.ExecuteScalarAsync(ct);
                    if (ptObj == null) return ActivateUserResult.UserNotFound;

                    currentPoints = Convert.ToInt32(ptObj);
                }

                // 3️⃣ UPDATE USER STATUS TO ACTIVE
                await using (var updateStatusCmd = conn.CreateCommand())
                {
                    updateStatusCmd.Transaction = tx;
                    updateStatusCmd.CommandText = @"
                UPDATE public.users
                SET status = 'Active'
                WHERE user_id = @UserId;
            ";

                    updateStatusCmd.Parameters.Add(new NpgsqlParameter("@UserId", request.user_id));
                    await updateStatusCmd.ExecuteNonQueryAsync(ct);
                }

                // 4️⃣ ADD 100 POINTS BACK
                int newPoints = currentPoints + 100;
                await using (var pointsCmd = conn.CreateCommand())
                {
                    pointsCmd.Transaction = tx;
                    pointsCmd.CommandText = @"
                UPDATE public.users
                SET total_points = @NewPoints
                WHERE user_id = @UserId;
            ";

                    pointsCmd.Parameters.Add(new NpgsqlParameter("@NewPoints", newPoints));
                    pointsCmd.Parameters.Add(new NpgsqlParameter("@UserId", request.user_id));

                    await pointsCmd.ExecuteNonQueryAsync(ct);
                }

                // 5️⃣ INSERT POINT HISTORY RECORD
                await using (var historyCmd = conn.CreateCommand())
                {
                    historyCmd.Transaction = tx;
                    historyCmd.CommandText = @"
                INSERT INTO public.pointshistory (user_id, points_change, reason)
                VALUES (@UserId, @Points, @Reason);
            ";

                    historyCmd.Parameters.Add(new NpgsqlParameter("@UserId", request.user_id));
                    historyCmd.Parameters.Add(new NpgsqlParameter("@Points", 100));
                    historyCmd.Parameters.Add(new NpgsqlParameter("@Reason", "User activated by SuperAdmin"));

                    await historyCmd.ExecuteNonQueryAsync(ct);
                }

                await tx.CommitAsync(ct);
                return ActivateUserResult.Success;
            }
            catch
            {
                await tx.RollbackAsync(ct);
                return ActivateUserResult.Failed;
            }
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
