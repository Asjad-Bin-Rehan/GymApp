using BS.Services.GymImagesService.DTOs;
using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BS.Services.GymImagesService
{
    public class GymImagesService : IGymImagesService
    {
        private readonly AppDbContext _dbContext;

        public GymImagesService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ---------------- ADD IMAGE ----------------
        public async Task<int> AddGymImage(AddGymImageDTO dto, CancellationToken ct)
        {
            if (dto.gym_id <= 0) throw new Exception("INVALID_GYM_ID");
            if (string.IsNullOrWhiteSpace(dto.image_url)) throw new Exception("INVALID_IMAGE_URL");

            await using var conn = (NpgsqlConnection)_dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            // Check if gym exists
            await using var checkCmd = new NpgsqlCommand(
                "SELECT COUNT(*) FROM partnergyms WHERE gym_id = @gym_id", conn);
            checkCmd.Parameters.AddWithValue("@gym_id", dto.gym_id);

            var count = (long)await checkCmd.ExecuteScalarAsync(ct);
            if (count == 0) throw new Exception("GYM_NOT_FOUND");

            // Insert image
            await using var cmd = new NpgsqlCommand(
                @"INSERT INTO gymimages (gym_id, image_url, added_at)
                  VALUES (@gym_id, @image_url, CURRENT_TIMESTAMP)
                  RETURNING image_id;", conn);
            cmd.Parameters.AddWithValue("@gym_id", dto.gym_id);
            cmd.Parameters.AddWithValue("@image_url", dto.image_url);

            var result = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt32(result);
        }

        // ---------------- GET IMAGES ----------------
        public async Task<List<ResponseGymImageDTO>> GetGymImages(int gymId, CancellationToken ct)
        {
            if (gymId <= 0) throw new Exception("INVALID_GYM_ID");

            await using var conn = (NpgsqlConnection)_dbContext.Database.GetDbConnection();
            await conn.OpenAsync(ct);

            // Check if gym exists
            await using var checkCmd = new NpgsqlCommand(
                "SELECT COUNT(*) FROM partnergyms WHERE gym_id = @gym_id", conn);
            checkCmd.Parameters.AddWithValue("@gym_id", gymId);

            var count = (long)await checkCmd.ExecuteScalarAsync(ct);
            if (count == 0) throw new Exception("GYM_NOT_FOUND");

            // Get images
            await using var cmd = new NpgsqlCommand(
                @"SELECT image_id, gym_id, image_url, added_at
                  FROM gymimages
                  WHERE gym_id = @gym_id
                  ORDER BY added_at DESC;", conn);
            cmd.Parameters.AddWithValue("@gym_id", gymId);

            var images = new List<ResponseGymImageDTO>();
            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                images.Add(new ResponseGymImageDTO
                {
                    image_id = reader.GetInt32(0),
                    gym_id = reader.GetInt32(1),
                    image_url = reader.GetString(2),
                    added_at = reader.GetDateTime(3)
                });
            }

            return images;
        }
    }
}
