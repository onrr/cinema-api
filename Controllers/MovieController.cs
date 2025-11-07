using Cinema.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class MovieController : ControllerBase
{
    private readonly DataContext _context;

    public MovieController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMovies()
    {
        var movies = await _context.Movies
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .Include(m => m.ShowTimes)
            .ToListAsync();

        var result = movies.Select(m => new
        {
            m.ID,
            m.Title,
            m.Description,
            m.PosterUrl,
            Genres = m.MovieGenres.Select(mg => mg.Genre?.Name).ToList(),
            ShowTimes = m.ShowTimes?.Select(st => st.StartTime).ToList()
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovie(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .Include(m => m.ShowTimes)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (movie == null) return NotFound();

        var result = new
        {
            movie.ID,
            movie.Title,
            movie.Description,
            movie.PosterUrl,
            Genres = movie.MovieGenres.Select(mg => mg.Genre?.Name).ToList(),
            ShowTimes = movie.ShowTimes?.Select(st => st.StartTime).ToList()
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie(CreateMovie dto)
    {

        var validGenreIds = await _context.Genres
            .Select(g => g.ID)
            .ToListAsync();

        var invalidIds = dto.GenreIDs
            .Where(id => !validGenreIds.Contains(id))
            .ToList();

        if (invalidIds.Any())
        {
            return BadRequest(new
            {
                Message = "One or more GenreIDs do not exist.",
                InvalidIDs = invalidIds
            });
        }

        var movie = new Movie
        {
            Title = dto.Title,
            Description = dto.Description,
            PosterUrl = dto.PosterUrl
        };

        foreach (var genreId in dto.GenreIDs)
            movie.MovieGenres.Add(new MovieGenre { GenreID = genreId });

        foreach (var time in dto.ShowTimes)
            movie.ShowTimes?.Add(new Showtime { StartTime = time });

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return Ok(movie);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, CreateMovie dto)
    {

        var validGenreIds = await _context.Genres
            .Select(g => g.ID)
            .ToListAsync();

        var invalidIds = dto.GenreIDs
            .Where(id => !validGenreIds.Contains(id))
            .ToList();

        if (invalidIds.Any())
        {
            return BadRequest(new
            {
                Message = "One or more GenreIDs do not exist.",
                InvalidIDs = invalidIds
            });
        }

        var movie = await _context.Movies
            .Include(m => m.MovieGenres)
            .Include(m => m.ShowTimes)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (movie == null) return NotFound();

        movie.Title = dto.Title;
        movie.Description = dto.Description;
        movie.PosterUrl = dto.PosterUrl;

        _context.MovieGenres.RemoveRange(movie.MovieGenres);
        foreach (var genreId in dto.GenreIDs)
            movie.MovieGenres.Add(new MovieGenre { GenreID = genreId });

        _context.Showtimes.RemoveRange(movie.ShowTimes!);
        foreach (var time in dto.ShowTimes)
            movie.ShowTimes?.Add(new Showtime { StartTime = time });

        await _context.SaveChangesAsync();
        return Ok(movie);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.MovieGenres)
            .Include(m => m.ShowTimes)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (movie == null) return NotFound();

        _context.MovieGenres.RemoveRange(movie.MovieGenres);
        _context.Showtimes.RemoveRange(movie.ShowTimes!);
        _context.Movies.Remove(movie);

        await _context.SaveChangesAsync();
        return Ok("Delete successfully");
    }

}
