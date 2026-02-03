using Microsoft.EntityFrameworkCore;
using ReservationApi.Data;
using ReservationApi.Domain;

namespace ReservationApi.Repositories;

class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _db;

    public ReservationRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Reservation>> GetAll(CancellationToken ct)
    {
        return await _db.Reservations.AsNoTracking().OrderBy(r => r.CheckIn).ToListAsync(ct);
    }

    public async Task<Reservation?> GetById(int id, CancellationToken ct)
    {
        return await _db.Reservations.FindAsync([id], ct);
    }

    public async Task<Reservation?> GetByReservationDetails(DateTime CheckIn, DateTime CheckOut, int roomId, CancellationToken ct)
    {
        var reservation = await _db.Reservations.FirstOrDefaultAsync(r =>
        r.CheckIn == CheckIn &&
        r.CheckOut == CheckOut,
        ct);
        await _db.SaveChangesAsync(ct);
        return reservation;
    }

    public async Task<Reservation> CreateReservation(Reservation reservation, CancellationToken ct)
    {
        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync(ct);
        return reservation;
    }

    public async Task<bool> UpdateAsync(Reservation reservation, CancellationToken ct)
    {
        var existing = await _db.Reservations.FirstOrDefaultAsync(r => r.Id == reservation.Id, ct);
        if (existing is null) return false;

        existing.CheckIn = reservation.CheckIn;
        existing.CheckOut = reservation.CheckOut;
        existing.Details = reservation.Details;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var existing = await _db.Reservations.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (existing is null) return false;

        _db.Reservations.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasOverlaps(DateTime CheckIn, DateTime CheckOut, int roomId, CancellationToken ct)
    {
        var overlapped = await _db.Reservations.AnyAsync(r => r.RoomId == roomId && CheckIn < r.CheckOut && CheckOut > r.CheckIn, ct);
        if (overlapped is false)
        {
            return overlapped;
        }
        return overlapped;
    }

}