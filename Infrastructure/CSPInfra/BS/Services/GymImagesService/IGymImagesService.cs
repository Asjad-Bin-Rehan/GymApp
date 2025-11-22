using BS.Services.GymImagesService.DTOs;

public interface IGymImagesService
{
    Task<int> AddGymImage(AddGymImageDTO dto, CancellationToken ct);
    Task<List<ResponseGymImageDTO>> GetGymImages(int gymId, CancellationToken ct);
}