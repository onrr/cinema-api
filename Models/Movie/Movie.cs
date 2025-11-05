using System.ComponentModel.DataAnnotations;

namespace Cinema.Models;

public class Movie
{
    public int ID { get; set; }
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = null!;
    public string? PosterUrl { get; set; } = null!;
    public List<MovieGenre> MovieGenres { get; set; } = new();
    public List<Showtime> ShowTimes { get; set; } = new();
}