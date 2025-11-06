namespace Cinema.Models;

public class Reservation
{
    public int ID { get; set; }
    public int ShowtimeID { get; set; }
    public Showtime? Showtime { get; set; }
    public string? UserEmail { get; set; }
    public string? FullName { get; set; }
    public int SeatNumber { get; set; }
}
