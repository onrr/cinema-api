
namespace Cinema.Models;

public class Movie
{
    public int ID { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? PosterUrl { get; set; } = null!;
    public List<MovieGenre> MovieGenres { get; set; } = new();
    public List<Showtime> ShowTimes { get; set; } = new();
}