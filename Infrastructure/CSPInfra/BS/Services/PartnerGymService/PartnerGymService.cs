using BS.Services.PartnerGymService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BS.Services.PartnerGymService
{
    public class PartnerGymService : IPartnerGymService
    {
        private readonly AppDbContext _dbContext;

        public PartnerGymService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // =======================================================
        // ADD PARTNER GYM
        // =======================================================
        public async Task<int> AddPartnerGymRaw(AddPartnerGymDTO request, int adminId, CancellationToken ct)
{
    await using var conn = _dbContext.Database.GetDbConnection();
    await conn.OpenAsync(ct);

    await using var transaction = await conn.BeginTransactionAsync(ct);

    try
    {
        // -----------------------------
        // 1️⃣ Insert Location
        // -----------------------------
        await using var locCmd = conn.CreateCommand();
        locCmd.Transaction = transaction;
        locCmd.CommandText = @"
            INSERT INTO locations (country, state, city, postal_code, address, latitude, longitude)
            VALUES (@country, @state, @city, @postal_code, @address, @latitude, @longitude)
            RETURNING location_id;
        ";

        locCmd.Parameters.Add(new NpgsqlParameter("@country", request.country));
        locCmd.Parameters.Add(new NpgsqlParameter("@state", request.state ?? (object)DBNull.Value));
        locCmd.Parameters.Add(new NpgsqlParameter("@city", request.city));
        locCmd.Parameters.Add(new NpgsqlParameter("@postal_code", request.postal_code ?? (object)DBNull.Value));
        locCmd.Parameters.Add(new NpgsqlParameter("@address", request.address ?? (object)DBNull.Value));
        locCmd.Parameters.Add(new NpgsqlParameter("@latitude", request.latitude));
        locCmd.Parameters.Add(new NpgsqlParameter("@longitude", request.longitude));

        var locationIdObj = await locCmd.ExecuteScalarAsync(ct);
        if (locationIdObj == null) throw new Exception("Failed to insert location.");

        var locationId = Convert.ToInt32(locationIdObj);

        // -----------------------------
        // 2️⃣ Insert Partner Gym
        // -----------------------------
        await using var gymCmd = conn.CreateCommand();
        gymCmd.Transaction = transaction;
        gymCmd.CommandText = @"
            INSERT INTO partnergyms 
                (gym_name, location_id, contact_person, phone, partnership_date, status, added_by)
            VALUES 
                (@gym_name, @location_id, @contact_person, @phone, CURRENT_DATE, 'Active', @added_by)
            RETURNING gym_id;
        ";

        gymCmd.Parameters.Add(new NpgsqlParameter("@gym_name", request.gym_name));
        gymCmd.Parameters.Add(new NpgsqlParameter("@location_id", locationId));
        gymCmd.Parameters.Add(new NpgsqlParameter("@contact_person", request.contact_person ?? (object)DBNull.Value));
        gymCmd.Parameters.Add(new NpgsqlParameter("@phone", request.phone ?? (object)DBNull.Value));
        gymCmd.Parameters.Add(new NpgsqlParameter("@added_by", adminId));

        var gymIdObj = await gymCmd.ExecuteScalarAsync(ct);
        if (gymIdObj == null) throw new Exception("Failed to insert partner gym.");

        var gymId = Convert.ToInt32(gymIdObj);

        await transaction.CommitAsync(ct);
        return gymId;
    }
    catch
    {
        await transaction.RollbackAsync(ct);
        throw;
    }
}



        public async Task<int> AddPartnerGymManual(AddPartnerGymManualDTO request, CancellationToken ct)
        {
            await using var conn = _dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            await using var trans = await conn.BeginTransactionAsync(ct);

            try
            {
                // 1️⃣ Insert Location
                var insertLocationCmd = conn.CreateCommand();
                insertLocationCmd.Transaction = trans;
                insertLocationCmd.CommandText = @"
                INSERT INTO public.locations
                    (country, state, city, postal_code, address, latitude, longitude)
                VALUES
                    (@country, @state, @city, @postal_code, @address, @latitude, @longitude)
                RETURNING location_id;
            ";

                insertLocationCmd.Parameters.Add(new NpgsqlParameter("@country", request.country));
                insertLocationCmd.Parameters.Add(new NpgsqlParameter("@state", request.state));
                insertLocationCmd.Parameters.Add(new NpgsqlParameter("@city", request.city));
                insertLocationCmd.Parameters.Add(new NpgsqlParameter("@postal_code", request.postal_code));
                insertLocationCmd.Parameters.Add(new NpgsqlParameter("@address", request.address));
                insertLocationCmd.Parameters.Add(new NpgsqlParameter("@latitude", request.latitude));
                insertLocationCmd.Parameters.Add(new NpgsqlParameter("@longitude", request.longitude));

                var locationId = (int)await insertLocationCmd.ExecuteScalarAsync(ct);

                // 2️⃣ Insert Partner Gym with admin_id
                var insertGymCmd = conn.CreateCommand();
                insertGymCmd.Transaction = trans;
                insertGymCmd.CommandText = @"
                INSERT INTO public.partnergyms
                    (gym_name, contact_person, phone, location_id, admin_id)
                VALUES
                    (@gym_name, @contact_person, @phone, @location_id, @admin_id)
                RETURNING gym_id;
            ";

                insertGymCmd.Parameters.Add(new NpgsqlParameter("@gym_name", request.gym_name));
                insertGymCmd.Parameters.Add(new NpgsqlParameter("@contact_person", request.contact_person));
                insertGymCmd.Parameters.Add(new NpgsqlParameter("@phone", request.phone));
                insertGymCmd.Parameters.Add(new NpgsqlParameter("@location_id", locationId));
                insertGymCmd.Parameters.Add(new NpgsqlParameter("@admin_id", request.admin_id));

                var gymId = (int)await insertGymCmd.ExecuteScalarAsync(ct);

                await trans.CommitAsync(ct);
                return gymId;
            }
            catch
            {
                await trans.RollbackAsync(ct);
                throw;
            }
        }



        // =======================================================
        // GET PARTNER GYM BY ID
        // =======================================================
        public async Task<ResponsePartnerGymDTO?> GetPartnerGymByIdRaw(int gymId, CancellationToken ct)
        {
            var sqlQuery = @"
                SELECT 
                    pg.gym_id, pg.gym_name, pg.location_id,
                    pg.contact_person, pg.phone, pg.partnership_date, pg.status,
                    l.latitude, l.longitude
                FROM public.partnergyms pg
                LEFT JOIN public.locations l ON pg.location_id = l.location_id
                WHERE pg.gym_id = @gym_id
            ";

            var parameter = new NpgsqlParameter("@gym_id", gymId);

            var result = await _dbContext.Database
                .SqlQueryRaw<ResponsePartnerGymDTO>(sqlQuery, parameter)
                .FirstOrDefaultAsync(ct);

            return result;
        }

        // =======================================================
        // LIST PARTNER GYMS
        // =======================================================
        public async Task<List<ResponsePartnerGymDTO>> ListPartnerGymsRaw(int limit, int offset, CancellationToken ct)
        {
            var sqlQuery = @"
                SELECT 
                    pg.gym_id, pg.gym_name, pg.location_id,
                    pg.contact_person, pg.phone, pg.partnership_date, pg.status,
                    l.latitude, l.longitude
                FROM public.partnergyms pg
                LEFT JOIN public.locations l ON pg.location_id = l.location_id
                ORDER BY pg.gym_id
                LIMIT @Limit OFFSET @Offset
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@Limit", limit),
                new NpgsqlParameter("@Offset", offset)
            };

            return await _dbContext.Database
                .SqlQueryRaw<ResponsePartnerGymDTO>(sqlQuery, parameters)
                .ToListAsync(ct);
        }

        // =======================================================
        // UPDATE PARTNER GYM
        // =======================================================
        public async Task<bool> UpdatePartnerGymRaw(UpdatePartnerGymDTO request, CancellationToken ct)
        {
            var sqlQuery = @"
                UPDATE public.partnergyms
                SET
                    gym_name = COALESCE(@gym_name, gym_name),
                    location_id = COALESCE(@location_id, location_id),
                    contact_person = COALESCE(@contact_person, contact_person),
                    phone = COALESCE(@phone, phone),
                    status = COALESCE(@status, status)
                WHERE gym_id = @gym_id
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@gym_id", request.gym_id),
                new NpgsqlParameter("@gym_name", request.gym_name ?? (object)DBNull.Value),
                new NpgsqlParameter("@location_id", request.location_id ?? (object)DBNull.Value),
                new NpgsqlParameter("@contact_person", request.contact_person ?? (object)DBNull.Value),
                new NpgsqlParameter("@phone", request.phone ?? (object)DBNull.Value),
                new NpgsqlParameter("@status", request.status ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, parameters, ct);
            return true;
        }

        // =======================================================
        // DELETE PARTNER GYM
        // =======================================================
        public async Task<bool> DeletePartnerGymRaw(int gymId, CancellationToken ct)
        {
            var sqlQuery = @"DELETE FROM public.partnergyms WHERE gym_id = @gym_id";

            var parameter = new NpgsqlParameter("@gym_id", gymId);

            await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, new object[] { parameter }, ct);

            return true;
        }
    }
}
