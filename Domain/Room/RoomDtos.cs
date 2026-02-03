namespace ReservationApi.Domain;

public record CreateRoomRequest(
    string Number,
    int Capacity,
    string Type
);

public record UpdateRoomRequest(
    string Number,
    int Capacity,
    string Type,
    bool IsActive
);

public record RoomResponse(
    Guid Id,
    string Number,
    int Capacity,
    string Type,
    bool IsActive
);