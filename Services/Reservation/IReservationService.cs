using ReservationApi.Domain;

namespace ReservationApi.Services;

public interface IReservationService
{
    Task<List<ReservationResponse>> GetAllAsync(CancellationToken ct);
    //GetSingle
    Task<ReservationResponse?> GetByIdAsync(Guid id, CancellationToken ct);
    //Create

    Task<(bool ok, string? error, Reservation? reservation)> CreateAsync(CreateReservationRequest req, CancellationToken ct);
    //Update
    Task<(bool ok, string? error)> UpdateAsync(Guid Id, UpdateReservationRequest req, CancellationToken ct);
    //Delete
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}

