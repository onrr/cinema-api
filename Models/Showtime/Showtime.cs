namespace Cinema.Models
{
    public class Showtime
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public int MovieID { get; set; }
        public Movie? Movie { get; set; }
        public int TheaterID { get; set; }
        public Theater? Theater { get; set; }
        public List<Reservation> Reservations { get; set; } = new();
    }
}
