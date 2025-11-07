
using Cinema.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheaterController : ControllerBase
    {
        private readonly DataContext _context;

        public TheaterController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetTheaters()
        {
            var today = DateTime.UtcNow.Date;
            var threeDaysAgo = today.AddDays(-3);

            var theaters = await _context.Theaters
                .Select(t => new
                {
                    t.ID,
                    t.Name,
                    t.Capacity,
                    ShowTimes = t.ShowTimes
                        .Where(s => s.StartTime.Date >= threeDaysAgo)
                        .OrderBy(s => s.StartTime)
                        .Select(s => new
                        {
                            s.StartTime,
                            MovieTitle = s.Movie!.Title
                        })
            .ToList()
                })
                .ToListAsync();

            if (theaters == null)
            {
                return NotFound();
            }

            return Ok(theaters);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTheater(int id)
        {
            var today = DateTime.UtcNow.Date;
            var threeDaysAgo = today.AddDays(-3);

            var theater = await _context.Theaters
                    .Where(t => t.ID == id)
                    .Select(t => new
                    {
                        t.ID,
                        t.Name,
                        t.Capacity,
                        ShowTimes = t.ShowTimes
                            .Where(s => s.StartTime.Date >= threeDaysAgo)
                            .OrderBy(s => s.StartTime)
                            .Select(s => new
                            {
                                s.StartTime,
                                MovieTitle = s.Movie!.Title
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

            if (theater == null)
            {
                return NotFound();
            }

            return Ok(theater);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CreateTheater(CreateTheater dto)
        {
            var theater = new Theater
            {
                Name = dto.Name,
                Capacity = dto.Capacity
            };
            _context.Theaters.Add(theater);
            await _context.SaveChangesAsync();

            return Ok(theater);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateTheater(int id, CreateTheater dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var theater = await _context.Theaters.FindAsync(id);

            if (theater == null)
            {
                return NotFound();
            }

            theater.Name = dto.Name;
            theater.Capacity = dto.Capacity;

            await _context.SaveChangesAsync();

            return Ok(theater);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteTheater(int id)
        {
            var theater = await _context.Theaters.FindAsync(id);

            if (theater == null)
            {
                return NotFound();
            }

            _context.Theaters.Remove(theater);
            await _context.SaveChangesAsync();

            return Ok("Delete successfully");
        }
    }
}