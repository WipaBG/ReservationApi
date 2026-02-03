using ReservationApi.Domain;
using ReservationApi.Repositories;

namespace ReservationApi.Services;

public class ReservationService(IReservationRepository reservationRepo, IRoomRepository roomRepo) : IReservationService
{
    private readonly IReservationRepository _reservation_repo = reservationRepo;
    private readonly IRoomRepository _room_repo = roomRepo;

    public async Task<List<ReservationResponse>> GetAllAsync(CancellationToken ct)
    {
        var reservations = await _reservation_repo.GetAll(ct);
        return reservations.Select(ToResponse).ToList();
    }

    public async Task<ReservationResponse?> GetByIdAsync(int Id, CancellationToken ct)
    {
        var reservation = await _reservation_repo.GetById(Id, ct);
        return reservation is null ? null : ToResponse(reservation);
    }

    public async Task<(bool ok, string? error, Reservation? reservation)> CreateAsync(CreateReservationRequest req, CancellationToken ct)
    {

        var existing = await _reservation_repo.GetByReservationDetails(req.CheckIn, req.CheckOut, req.RoomId, ct);

        if (existing is not null)
        {
            return (false, "Reservation already exists", null);
        }

        var (status, error) = await ValidateReservation(req.RoomId, req.CheckIn, req.CheckOut, ct);

        if (status == false)
        {
            return (false, error, null);
        }

        var reservation = new Reservation
        {
            RoomId = req.RoomId,
            Details = req.Details,
            CheckIn = req.CheckIn,
            CheckOut = req.CheckOut,
        };


        var rsrv = await _reservation_repo.CreateReservation(reservation, ct);

        return (true, null, rsrv);
    }

    public async Task<(bool ok, string? error)> UpdateAsync(int Id, UpdateReservationRequest req, CancellationToken ct)
    {
        var reservation = await _reservation_repo.GetById(Id, ct);

        if (reservation is null)
        {
            return (false, "reservation does not exist");
        }

        var (status, error) = await ValidateReservation(req.RoomId, req.CheckIn, req.CheckOut, ct);

        if (status == false)
        {
            return (false, error);
        }

        reservation.RoomId = req.RoomId;
        reservation.Details = req.Details;
        reservation.CheckIn = req.CheckIn;
        reservation.CheckOut = req.CheckOut;

        var updated = await _reservation_repo.UpdateAsync(reservation, ct);
        return updated ? (true, null) : (false, "error");
    }

    public async Task<bool> DeleteAsync(int Id, CancellationToken ct)
    {
        return await _reservation_repo.DeleteAsync(Id, ct);
    }

    private async Task<(bool status, string? error)> ValidateReservation(int roomId, DateTime CheckIn, DateTime CheckOut, CancellationToken ct)
    {
        if (roomId <= 0)
        {
            return (false, "Room ID is required");
        }

        var room = await _room_repo.GetByIdAsync(roomId, ct);

        if (room is null)
        {
            return (false, "Room not found");
        }

        if (CheckIn >= CheckOut)
        {
            return (false, "Reservation check-in should be before check-out");
        }

        var overlapped = await _reservation_repo.FindOverlaps(CheckIn, CheckOut, roomId, ct);

        if (overlapped)
        {
            return (false, "Room is already reserved for this period");
        }

        return (true, null);
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

