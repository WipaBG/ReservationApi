namespace ReservationApi.Domain;

public record CreateReservationRequest
(
    DateTime CheckIn,
    DateTime CheckOut,
    string RoomNumber,
    string Details
);

public record UpdateReservationRequest
(
    DateTime CheckIn,
    DateTime CheckOut,
    string RoomNumber,
    string Details,
    ReservationStatus Status
);



public record ReservationResponse(
    Guid Id,
    DateTime CheckIn,
    DateTime CheckOut,
    string RoomNumber,
    string Details,
    ReservationStatus Status
);