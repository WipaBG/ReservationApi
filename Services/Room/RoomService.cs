using ReservationApi.Domain;
using ReservationApi.Repositories;

namespace ReservationApi.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _repo;

    public RoomService(IRoomRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<RoomResponse>> GetAllAsync(CancellationToken ct)
    {
        var rooms = await _repo.GetAllAsync(ct);
        return rooms.Select(ToResponse).ToList();
    }

    public async Task<RoomResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var room = await _repo.GetByIdAsync(id, ct);
        return room is null ? null : ToResponse(room);
    }

    public async Task<(bool ok, string? error, RoomResponse? room)> CreateAsync(CreateRoomRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Number))
        {
            return (false, "Room number is required", null);
        }

        if (req.Capacity <= 0)
        {
            return (false, "Room number is required", null);
        }

        var existing = await _repo.GetByNumberAsync(req.Number, ct);

        if (existing is not null)
        {
            return (false, $"Room number '{req.Number}' already exists", null);
        }

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Number = req.Number.Trim(),
            Capacity = req.Capacity,
            Type = req.Type.Trim(),
        };

        var created = await _repo.CreateAsync(room, ct);
        return (true, null, ToResponse(created));
    }

    public async Task<(bool ok, string? error)> UpdateAsync(Guid id, UpdateRoomRequest req, CancellationToken ct)
    {
        var room = await _repo.GetByIdAsync(id, ct);
        if (room is null) return (false, "Room not found");

        if (string.IsNullOrWhiteSpace(req.Number)) return (false, "Room number is required");

        if (req.Capacity <= 0) return (false, "Capacity must be > 0");

        if (!string.Equals(room.Number, req.Number, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _repo.GetByNumberAsync(req.Number, ct);
            if (existing is not null) return (false, $"Room number '{req.Number}' already exists");

        }

        room.Number = req.Number.Trim();
        room.Capacity = req.Capacity;
        room.Type = req.Type.Trim();

        var updated = await _repo.UpdateAsync(room, ct);
        return updated ? (true, null) : (false, "Update failed");
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        return _repo.DeleteAsync(id, ct);
    }

    private static RoomResponse ToResponse(Room r) => new(r.Id, r.Number, r.Capacity, r.Type);

}