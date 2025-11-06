using System.ComponentModel.DataAnnotations;

namespace Cinema.Models;

public class CreateReservation
{
    [Required]
    public int ShowTimeID { get; set; }
    [Required]
    public int FullName { get; set; }
    [Required]
    public int SeatNumber { get; set; }
}
