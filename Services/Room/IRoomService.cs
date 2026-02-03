using ReservationApi.Domain;

namespace ReservationApi.Services;

public interface IRoomService
{
    Task<List<RoomResponse>> GetAllAsync(CancellationToken ct);
    Task<RoomResponse?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<(bool ok, string? error, RoomResponse? room)> CreateAsync(CreateRoomRequest req, CancellationToken ct);
    Task<(bool ok, string? error)> UpdateAsync(Guid id, UpdateRoomRequest req, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct); 
}