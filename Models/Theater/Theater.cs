
namespace Cinema.Models
{
    public class Theater
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public List<Showtime> ShowTimes { get; set; } = new();
    }
}