using BS.Services.LocationService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BS.Services.LocationService
{
    public class LocationService : ILocationService
    {
        private readonly AppDbContext _dbContext;

        public LocationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // =============================
        // CREATE
        // =============================
        public async Task<int> AddLocationRaw(AddLocationDTO request, CancellationToken ct)
        {
            var sql = @"
                INSERT INTO public.locations
                    (country, state, city, postal_code, address, latitude, longitude)
                VALUES
                    (@country, @state, @city, @postal_code, @address, @latitude, @longitude)
                RETURNING location_id
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@country", request.country ?? (object)DBNull.Value),
                new NpgsqlParameter("@state", request.state ?? (object)DBNull.Value),
                new NpgsqlParameter("@city", request.city ?? (object)DBNull.Value),
                new NpgsqlParameter("@postal_code", request.postal_code ?? (object)DBNull.Value),
                new NpgsqlParameter("@address", request.address ?? (object)DBNull.Value),
                new NpgsqlParameter("@latitude", request.latitude ?? (object)DBNull.Value),
                new NpgsqlParameter("@longitude", request.longitude ?? (object)DBNull.Value)
            };

            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, ct);
        }

        // =============================
        // READ BY ID
        // =============================
        public async Task<ResponseLocationDTO?> GetLocationByIdRaw(int locationId, CancellationToken ct)
        {
            var sql = @"
                SELECT location_id, country, state, city, postal_code, address, latitude, longitude
                FROM public.locations
                WHERE location_id = @location_id
            ";

            var param = new NpgsqlParameter("@location_id", locationId);

            return await _dbContext
                .Database
                .SqlQueryRaw<ResponseLocationDTO>(sql, param)
                .FirstOrDefaultAsync(ct);
        }

        // =============================
        // LIST / PAGINATION
        // =============================
        public async Task<List<ResponseLocationDTO>> ListAllLocationsRaw(int limit, int offset, CancellationToken ct)
        {
            var sql = @"
                SELECT location_id, country, state, city, postal_code, address, latitude, longitude
                FROM public.locations
                ORDER BY location_id
                LIMIT @limit OFFSET @offset
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@limit", limit),
                new NpgsqlParameter("@offset", offset)
            };

            return await _dbContext
                .Database
                .SqlQueryRaw<ResponseLocationDTO>(sql, parameters)
                .ToListAsync(ct);
        }

        // =============================
        // UPDATE
        // =============================
        public async Task<bool> UpdateLocationRaw(UpdateLocationDTO request, CancellationToken ct)
        {
            var sql = @"
                UPDATE public.locations
                SET
                    country = @country,
                    state = @state,
                    city = @city,
                    postal_code = @postal_code,
                    address = @address,
                    latitude = @latitude,
                    longitude = @longitude
                WHERE location_id = @location_id
            ";

            var parameters = new[]
            {
                new NpgsqlParameter("@location_id", request.location_id),
                new NpgsqlParameter("@country", request.country ?? (object)DBNull.Value),
                new NpgsqlParameter("@state", request.state ?? (object)DBNull.Value),
                new NpgsqlParameter("@city", request.city ?? (object)DBNull.Value),
                new NpgsqlParameter("@postal_code", request.postal_code ?? (object)DBNull.Value),
                new NpgsqlParameter("@address", request.address ?? (object)DBNull.Value),
                new NpgsqlParameter("@latitude", request.latitude ?? (object)DBNull.Value),
                new NpgsqlParameter("@longitude", request.longitude ?? (object)DBNull.Value)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, ct);
            return true;
        }

        // =============================
        // DELETE
        // =============================
        public async Task<bool> DeleteLocationRaw(int locationId, CancellationToken ct)
        {
            var sql = @"
                DELETE FROM public.locations
                WHERE location_id = @location_id
            ";

            var param = new NpgsqlParameter("@location_id", locationId);

            await _dbContext.Database.ExecuteSqlRawAsync(sql, param, ct);
            return true;
        }
    }
}
