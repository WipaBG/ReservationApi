namespace ReservationApi.Domain;

public class Room
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Number { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

}