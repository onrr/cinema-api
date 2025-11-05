using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Showtime
    {
        public int ID { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public int MovieID { get; set; }
        public Movie? Movie { get; set; }
    }
}
