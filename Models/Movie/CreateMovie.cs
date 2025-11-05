using System.ComponentModel.DataAnnotations;

namespace Cinema.Models;

public class CreateMovie
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = null!;
    public string? PosterUrl { get; set; } = null!;
    public List<int> GenreIDs { get; set; } = new();
    public List<DateTime> ShowTimes { get; set; } = new();
}