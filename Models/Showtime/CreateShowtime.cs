using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class CreateShowtime
    {
        [Required]
        public int MovieID { get; set; }

        [Required]
        public int TheaterID { get; set; }

        [Required]
        public DateTime StartTime { get; set; }
    }
}
