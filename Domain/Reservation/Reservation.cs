namespace ReservationApi.Domain;

public class Reservation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string RoomNumber { get; set; } = "";
    public string Details { get; set; } = string.Empty;
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
}

public enum ReservationStatus
{
    Confirmed,
    Pending,
    Cancelled
}