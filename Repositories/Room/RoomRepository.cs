using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using ReservationApi.Data;
using ReservationApi.Domain;

namespace ReservationApi.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _db;

    public RoomRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Room>> GetAllAsync(CancellationToken ct)
    {
        return await _db.Rooms.AsNoTracking().OrderBy(r => r.Number).ToListAsync(ct);
    }

    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Rooms.FindAsync(new object[] { id }, ct);
    }

    public async Task<Room?> GetByNumberAsync(string number, CancellationToken ct)
    {
        var n = number.Trim();

        return await _db.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(
                r => r.Number == n,
                ct
            );
    }

    public async Task<Room> CreateAsync(Room room, CancellationToken ct)
    {
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync(ct);
        return room;
    }

    public async Task<bool> UpdateAsync(Room room, CancellationToken ct)
    {
        var existing = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == room.Id, ct);
        if (existing is null) return false;
        existing.Number = room.Number;
        existing.Capacity = room.Capacity;
        existing.Type = room.Type;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var existing = await _db.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (existing is null) return false;

        _db.Rooms.Remove(existing);
        await _db.SaveChangesAsync(ct);
        return true;
    }


}