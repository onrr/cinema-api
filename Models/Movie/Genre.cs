

namespace Cinema.Models
{
    public class Genre
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public List<MovieGenre> MovieGenres { get; set; } = new();
    }
}
