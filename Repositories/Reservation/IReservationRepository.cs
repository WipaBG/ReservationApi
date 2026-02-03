namespace ReservationApi.Domain;

public interface IReservationRepository
{
    Task<List<Reservation>> GetAll(CancellationToken ct);
    Task<Reservation?> GetById(Guid id, CancellationToken ct);
    Task<Reservation?> GetByReservationDetails(DateTime CheckIn, DateTime CheckOut, string room, CancellationToken ct);

    Task<Reservation> CreateReservation(Reservation reservation, CancellationToken ct);
    Task<bool> UpdateAsync(Reservation reservation, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}