using Cinema.Models;
using Microsoft.EntityFrameworkCore;

public class ReservationCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReservationCleanupService> _logger;

    public ReservationCleanupService(IServiceScopeFactory scopeFactory, ILogger<ReservationCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            var now = DateTime.UtcNow;
            var oldReservations = await context.Reservations
                .Include(r => r.Showtime)
                .Where(r => r.Status == "Cancelled" && r.Showtime!.StartTime <= now)
                .ToListAsync(stoppingToken);

            if (oldReservations.Any())
            {
                context.Reservations.RemoveRange(oldReservations);
                await context.SaveChangesAsync(stoppingToken);

                _logger.LogInformation($"{oldReservations.Count} cancelled reservations deleted at {DateTime.UtcNow}");
            }

            await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
        }
    }
}
