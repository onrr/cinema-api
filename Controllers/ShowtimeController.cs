
using Cinema.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowtimeController : ControllerBase
    {
        private readonly DataContext _context;

        public ShowtimeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowtimes()
        {
            var showtimes = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .Select(s => new
                {
                    s.ID,
                    MovieTitle = s.Movie!.Title,
                    TheaterName = s.Theater!.Name,
                    s.StartTime,
                })
                .ToListAsync();

            return Ok(showtimes);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowtime(int id)
        {
            var showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .Where(s => s.ID == id)
                .Select(s => new
                {
                    s.ID,
                    MovieTitle = s.Movie!.Title,
                    TheaterName = s.Theater!.Name,
                    s.StartTime
                })
                .FirstOrDefaultAsync();

            if (showtime == null)
                return NotFound();

            return Ok(showtime);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CreateShowtime(CreateShowtime dto)
        {
            var movieExists = await _context.Movies.AnyAsync(m => m.ID == dto.MovieID);
            var theaterExists = await _context.Theaters.AnyAsync(t => t.ID == dto.TheaterID);

            if (!movieExists || !theaterExists)
                return BadRequest("Movie or Theater does not exist.");

            var threeHoursBefore = dto.StartTime.AddHours(-3);
            var threeHoursAfter = dto.StartTime.AddHours(3);

            bool showtimeControl = await _context.Showtimes
                .AnyAsync(s =>
                    s.TheaterID == dto.TheaterID &&
                    s.StartTime >= threeHoursBefore &&
                    s.StartTime <= threeHoursAfter
                );

            if (showtimeControl)
            {
                return Conflict(new { message = "There is another screening scheduled in this theater at the selected date and time." });
            }

            var showtime = new Showtime
            {
                MovieID = dto.MovieID,
                TheaterID = dto.TheaterID,
                StartTime = dto.StartTime
            };

            _context.Showtimes.Add(showtime);
            await _context.SaveChangesAsync();

            var result = await _context.Showtimes
                .Where(s => s.ID == showtime.ID)
                .Select(s => new
                {
                    s.ID,
                    s.StartTime,
                    movie = s.Movie!.Title,
                    theater = s.Theater!.Name
                })
                .FirstOrDefaultAsync();

            return Ok(result);
        }


        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateShowtime(int id, CreateShowtime dto)
        {

            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime == null)
                return NotFound();

            var movieExists = await _context.Movies.AnyAsync(m => m.ID == dto.MovieID);
            var theaterExists = await _context.Theaters.AnyAsync(t => t.ID == dto.TheaterID);

            if (!movieExists || !theaterExists)
                return BadRequest("Movie or Theater does not exist.");


            var threeHoursBefore = dto.StartTime.AddHours(-3);
            var threeHoursAfter = dto.StartTime.AddHours(3);

            bool showtimeControl = await _context.Showtimes
                .AnyAsync(s =>
                    s.TheaterID == dto.TheaterID &&
                    s.ID != id &&
                    s.StartTime >= threeHoursBefore &&
                    s.StartTime <= threeHoursAfter
                );

            if (showtimeControl)
            {
                return Conflict(new { message = "There is another screening scheduled in this theater at the selected date and time." });
            }

            showtime.MovieID = dto.MovieID;
            showtime.TheaterID = dto.TheaterID;
            showtime.StartTime = dto.StartTime;

            await _context.SaveChangesAsync();

            var result = await _context.Showtimes
                .Where(s => s.ID == showtime.ID)
                .Select(s => new
                {
                    s.ID,
                    s.StartTime,
                    movie = s.Movie!.Title,
                    theater = s.Theater!.Name
                })
                .FirstOrDefaultAsync();

            return Ok(result);
        }


        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteShowtime(int id)
        {
            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime == null)
                return NotFound();

            _context.Showtimes.Remove(showtime);
            await _context.SaveChangesAsync();

            return Ok("Delete successfully");
        }
    }
}