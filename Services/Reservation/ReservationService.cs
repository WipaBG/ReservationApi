using ReservationApi.Domain;

namespace ReservationApi.Services;

public class ReservationService(IReservationRepository repo) : IReservationService
{
    private readonly IReservationRepository _repo = repo;

    public async Task<List<ReservationResponse>> GetAllAsync(CancellationToken ct)
    {
        var reservations = await _repo.GetAll(ct);
        return reservations.Select(ToResponse).ToList();
    }

    public async Task<ReservationResponse?> GetByIdAsync(int Id, CancellationToken ct)
    {
        var reservation = await _repo.GetById(Id, ct);
        return reservation is null ? null : ToResponse(reservation);
    }

    public async Task<(bool ok, string? error, Reservation? reservation)> CreateAsync(CreateReservationRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.RoomNumber))
        {
            return (false, "Room number is required", null);
        }

        if (req.CheckIn >= req.CheckOut)
        {
            return (false, "Reservation check-in should be before check-out", null);
        }

        var existing = await _repo.GetByReservationDetails(req.CheckIn, req.CheckOut, req.RoomNumber, ct);

        if (existing is not null)
        {
            return (false, "Reservation already exists", null);
        }


        var reservation = new Reservation
        {
            RoomId = req.RoomId,
            Details = req.Details,
            CheckIn = req.CheckIn,
            CheckOut = req.CheckOut,
        };


        var rsrv = await _repo.CreateReservation(reservation, ct);

        return (true, null, rsrv);
    }

    public async Task<(bool ok, string? error)> UpdateAsync(int Id, UpdateReservationRequest req, CancellationToken ct)
    {
        var reservation = await _repo.GetById(Id, ct);

        if (reservation is null)
        {
            return (false, "reservation does not exist");
        }

        if (string.IsNullOrWhiteSpace(req.RoomNumber))
        {
            return (false, "room number is required");
        }

        if (req.CheckIn >= req.CheckOut)
        {
            return (false, "check-in date should be before check-out date");
        }

        reservation.RoomId = req.RoomId;
        reservation.CheckIn = req.CheckIn;
        reservation.CheckOut = req.CheckOut;

        var updated = await _repo.UpdateAsync(reservation, ct);
        return updated ? (true, null) : (false, "error");
    }

    public async Task<bool> DeleteAsync(int Id, CancellationToken ct)
    {
        return await _repo.DeleteAsync(Id, ct);
    }




    private static ReservationResponse ToResponse(Reservation rsrv)
    {
        return new ReservationResponse(
            rsrv.Id,
            rsrv.CheckIn,
            rsrv.CheckOut,
            rsrv.RoomId,
            rsrv.Details
        );
    }

}

