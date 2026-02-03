using ReservationApi.Domain;

namespace ReservationApi.Repositories;

public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync(CancellationToken ct);
    Task<Room?> GetByIdAsync(int Id, CancellationToken ct);
    Task<Room?> GetByNumberAsync(string number, CancellationToken ct);

    Task<Room> CreateAsync(Room room, CancellationToken ct);
    Task<bool> UpdateAsync(Room room, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}