namespace ReservationApi.Domain;

public class Room
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Type { get; set; } = string.Empty;
    public List<Reservation> Reservations { get; set; } = [];

}