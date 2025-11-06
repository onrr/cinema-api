using Cinema.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReservationController : ControllerBase
    {
        private readonly DataContext _context;

        public ReservationController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyReservations()
        {
            var userEmail = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                ?.Value;

            if (string.IsNullOrEmpty(userEmail))
                return BadRequest("User email not found in token.");


            var reservations = await _context.Reservations
                .Include(r => r.Showtime)
                    .ThenInclude(s => s!.Movie)
                .Include(r => r.Showtime)
                    .ThenInclude(s => s!.Theater)
                .Where(r => r.UserEmail == userEmail)
                .Select(r => new
                {
                    r.ID,
                    r.SeatNumber,
                    r.UserEmail,
                    r.FullName,
                    ShowTimeId = r.ShowtimeID,
                    Movie = r!.Showtime!.Movie!.Title,
                    Theater = r.Showtime.Theater!.Name,
                    r.Showtime.StartTime
                })
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation(int id)
        {
            var userEmail = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                ?.Value;

            if (string.IsNullOrEmpty(userEmail))
                return BadRequest("User email not found in token.");

            var reservation = await _context.Reservations
                .Include(r => r.Showtime)
                    .ThenInclude(s => s!.Movie)
                .Include(r => r.Showtime)
                    .ThenInclude(s => s!.Theater)
                .Where(r => r.ID == id && r.UserEmail == userEmail)
                .Select(r => new
                {
                    r.ID,
                    r.SeatNumber,
                    r.UserEmail,
                    r.FullName,
                    ShowTimeId = r.ShowtimeID,
                    Movie = r!.Showtime!.Movie!.Title,
                    Theater = r.Showtime.Theater!.Name,
                    r.Showtime.StartTime
                })
                .FirstOrDefaultAsync();

            if (reservation == null)
                return NotFound("Reservation not found or access denied.");

            return Ok(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservation dto)
        {
            var userEmail = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
                ?.Value;

            var fullName = User.Claims
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            if (string.IsNullOrEmpty(userEmail))
                return BadRequest("User email not found in token.");

            var showtime = await _context.Showtimes
                .Include(s => s.Theater)
                .Include(s => s.Reservations)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(s => s.ID == dto.ShowTimeID);

            if (showtime == null)
                return BadRequest("Invalid.");


            var theater = showtime.Theater!;
            int seatNumber = dto.SeatNumber;

            if (seatNumber < 1 || seatNumber > theater.Capacity)
            {
                return BadRequest($"Invalid seat number. Seat number must be between 1 and {theater.Capacity}.");
            }

            bool seatTaken = await _context.Reservations
                .AnyAsync(r => r.ShowtimeID == dto.ShowTimeID && r.SeatNumber == seatNumber);

            if (seatTaken)
                return BadRequest($"The selected seat number {seatNumber} is already reserved.");

            var reservation = new Reservation
            {
                ShowtimeID = dto.ShowTimeID,
                UserEmail = userEmail,
                FullName = fullName,
                SeatNumber = dto.SeatNumber
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                reservation.ID,
                reservation.FullName,
                reservation.SeatNumber,
                Movie = showtime.Movie!.Title,
                Theater = showtime.Theater!.Name,
                showtime.StartTime
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var userEmail = User.Claims
                .FirstOrDefault(c => c.Type == "emailaddress")
                ?.Value;

            if (string.IsNullOrEmpty(userEmail))
                return BadRequest("User email not found in token.");

            var reservation = await _context.Reservations
                .Include(r => r.Showtime)
                    .ThenInclude(s => s!.Movie)
                .Include(r => r.Showtime!.Theater)
                .FirstOrDefaultAsync(r => r.ID == id && r.UserEmail == userEmail);

            if (reservation == null)
                return NotFound("Reservation not found or you are not authorized to cancel it.");

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Reservation successfully canceled.",
                ReservationID = reservation.ID,
                Email = reservation.UserEmail,
                FullName = reservation.FullName,
                Movie = reservation.Showtime!.Movie!.Title,
                Theater = reservation.Showtime.Theater!.Name,
                reservation.SeatNumber,
                reservation.Showtime.StartTime
            });
        }

    }
}