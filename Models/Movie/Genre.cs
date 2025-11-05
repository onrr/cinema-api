using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Genre
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public List<MovieGenre> MovieGenres { get; set; } = new();
    }
}
