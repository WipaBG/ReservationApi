namespace ReservationApi.Domain;

public record CreateReservationRequest
(
    DateTime CheckIn,
    DateTime CheckOut,
    int RoomId,
    string Details
);

public record UpdateReservationRequest
(
    int RoomId,
    DateTime CheckIn,
    DateTime CheckOut,
    string RoomNumber,
    string Details
);



public record ReservationResponse(
    int Id,
    DateTime CheckIn,
    DateTime CheckOut,
    int RoomId,
    string Details
);