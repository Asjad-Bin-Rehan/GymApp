using BS.Services.LocationService.DTOs;

namespace BS.Services.LocationService
{
    public interface ILocationService
    {
        Task<int> AddLocationRaw(AddLocationDTO request, CancellationToken ct);

        Task<ResponseLocationDTO?> GetLocationByIdRaw(int locationId, CancellationToken ct);

        Task<List<ResponseLocationDTO>> ListAllLocationsRaw(int limit, int offset, CancellationToken ct);

        Task<bool> UpdateLocationRaw(UpdateLocationDTO request, CancellationToken ct);

        Task<bool> DeleteLocationRaw(int locationId, CancellationToken ct);
    }
}
