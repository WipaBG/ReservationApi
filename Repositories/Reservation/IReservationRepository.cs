namespace ReservationApi.Domain;

public interface IReservationRepository
{
    Task<List<Reservation>> GetAll(CancellationToken ct);
    Task<Reservation?> GetById(int id, CancellationToken ct);
    Task<Reservation?> GetByReservationDetails(DateTime CheckIn, DateTime CheckOut, int roomId, CancellationToken ct);

    Task<Reservation> CreateReservation(Reservation reservation, CancellationToken ct);
    Task<bool> UpdateAsync(Reservation reservation, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);

    Task<bool> FindOverlaps(DateTime CheckIn, DateTime CheckOut, int roomId, CancellationToken ct);
}