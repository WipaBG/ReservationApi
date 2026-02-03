namespace ReservationApi.Domain;

public class Reservation
{
    public int Id { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string Details { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public Room? Room { get; set; }
}

