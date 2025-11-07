
using Microsoft.AspNetCore.Mvc;
using Cinema.Services;
using Cinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly DataContext _context;

    public AdminController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("theaters")]
    public async Task<IActionResult> GetAllTheaters()
    {
        var theaters = await _context.Theaters
            .Select(t => new
            {
                t.ID,
                t.Name,
                t.Capacity,
                ShowTimes = t.ShowTimes
                .Select(s => new
                {
                    s.ID,
                    s.StartTime,
                    MovieTitle = s.Movie!.Title
                }).ToList()
            })
        .ToListAsync();

        return Ok(theaters);
    }

    [HttpGet("theater/{id}")]
    public async Task<IActionResult> GetTheater(int id)
    {

        var theater = await _context.Theaters
            .Where(t => t.ID == id)
            .Select(t => new
            {
                t.ID,
                t.Name,
                t.Capacity,
                ShowTimes = t.ShowTimes
                    .Select(s => new
                    {
                        s.ID,
                        s.StartTime,
                        MovieTitle = s.Movie!.Title
                    }).ToList()
            })
            .FirstOrDefaultAsync();

        if (theater == null)
            return NotFound();

        return Ok(theater);
    }

    [HttpGet("showtimes")]
    public async Task<IActionResult> GetAllShowtimes()
    {
        var showtimes = await _context.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Theater)
            .Select(s => new
            {
                s.ID,
                MovieTitle = s.Movie!.Title,
                TheaterName = s.Theater!.Name,
                s.StartTime
            })
            .ToListAsync();

        return Ok(showtimes);
    }

    [HttpGet("showtime/{id}")]
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

    [HttpGet("reservations")]
    public async Task<IActionResult> GetAllReservations()
    {
        var reservations = await _context.Reservations
            .Include(r => r.Showtime)
                .ThenInclude(s => s!.Movie)
            .Include(r => r.Showtime)
                .ThenInclude(s => s!.Theater)
            .Select(r => new
            {
                r.ID,
                r.UserEmail,
                ShowtimeID = r.ShowtimeID,
                MovieTitle = r.Showtime!.Movie!.Title,
                TheaterName = r.Showtime.Theater!.Name,
                r.SeatNumber
            })
            .ToListAsync();

        return Ok(reservations);
    }

    [HttpGet("reservation/{id}")]
    public async Task<IActionResult> GetReservation(int id)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Showtime)
                .ThenInclude(s => s!.Movie)
            .Include(r => r.Showtime)
                .ThenInclude(s => s!.Theater)
            .Where(r => r.ID == id)
            .Select(r => new
            {
                r.ID,
                r.UserEmail,
                ShowtimeID = r.ShowtimeID,
                MovieTitle = r.Showtime!.Movie!.Title,
                TheaterName = r.Showtime.Theater!.Name,
                r.SeatNumber
            })
            .FirstOrDefaultAsync();

        if (reservation == null)
            return NotFound();

        return Ok(reservation);
    }

}