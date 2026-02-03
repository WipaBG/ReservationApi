using ReservationApi.Domain;

namespace ReservationApi.Services;

public interface IRoomService
{
    Task<List<RoomResponse>> GetAllAsync(CancellationToken ct);
    Task<RoomResponse?> GetByIdAsync(int id, CancellationToken ct);
    Task<(bool ok, string? error, RoomResponse? room)> CreateAsync(CreateRoomRequest req, CancellationToken ct);
    Task<(bool ok, string? error)> UpdateAsync(int id, UpdateRoomRequest req, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}