using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Models;

public class DataContext : IdentityDbContext<AppUser, AppRole, int>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Showtime> Showtimes { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<Theater> Theaters { get; set; }
    public DbSet<Reservation> Reservations { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Movie - Genre
        builder.Entity<MovieGenre>()
            .HasKey(mg => new { mg.MovieID, mg.GenreID });

        builder.Entity<MovieGenre>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieID);

        builder.Entity<MovieGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenres)
            .HasForeignKey(mg => mg.GenreID);

        // Theatre - Movie - showtime
        builder.Entity<Showtime>()
            .HasOne(s => s.Movie)
            .WithMany(m => m.ShowTimes)
            .HasForeignKey(s => s.MovieID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Showtime>()
            .HasOne(s => s.Theater)
            .WithMany(t => t.ShowTimes)
            .HasForeignKey(s => s.TheaterID)
            .OnDelete(DeleteBehavior.Restrict);

        // Genre
        builder.Entity<Genre>().HasData(
            new Genre { ID = 1, Name = "Action" },
            new Genre { ID = 2, Name = "Sci-Fi" },
            new Genre { ID = 3, Name = "Drama" },
            new Genre { ID = 4, Name = "Comedy" }
            );

        // Movie
        builder.Entity<Movie>().HasData(
            new Movie
            {
                ID = 1,
                Title = "The Shawshank Redemption",
                Description = "A banker convicted of uxoricide forms a friendship over a quarter century with a hardened convict, while maintaining his innocence and trying to remain hopeful through simple compassion.",
                PosterUrl = "https://m.media-amazon.com/images/M/MV5BMDAyY2FhYjctNDc5OS00MDNlLThiMGUtY2UxYWVkNGY2ZjljXkEyXkFqcGc@._V1_FMjpg_UX1200_.jpg"
            },
            new Movie
            {
                ID = 2,
                Title = "The Godfather",
                Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                PosterUrl = "https://m.media-amazon.com/images/M/MV5BNGEwYjgwOGQtYjg5ZS00Njc1LTk2ZGEtM2QwZWQ2NjdhZTE5XkEyXkFqcGc@._V1_.jpg"
            },
            new Movie
            {
                ID = 3,
                Title = "The Dark Knight",
                Description = "When a menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman, James Gordon and Harvey Dent must work together to put an end to the madness.",
                PosterUrl = "https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_FMjpg_UX1000_.jpg"
            },
            new Movie
            {
                ID = 4,
                Title = "Interstellar",
                Description = "When Earth becomes uninhabitable in the future, a farmer and ex-NASA pilot, Joseph Cooper, is tasked to pilot a spacecraft, along with a team of researchers, to find a new planet for humans.",
                PosterUrl = "https://m.media-amazon.com/images/M/MV5BYzdjMDAxZGItMjI2My00ODA1LTlkNzItOWFjMDU5ZDJlYWY3XkEyXkFqcGc@._V1_.jpg"
            },
            new Movie
            {
                ID = 5,
                Title = "Toy Story 3",
                Description = "The toys are mistakenly delivered to a day-care center instead of the attic right before Andy leaves for college, and it's up to Woody to convince the other toys that they weren't abandoned and to return home.",
                PosterUrl = "https://m.media-amazon.com/images/M/MV5BMTgxOTY4Mjc0MF5BMl5BanBnXkFtZTcwNTA4MDQyMw@@._V1_FMjpg_UX1000_.jpg"
            }
        );

        builder.Entity<MovieGenre>().HasData(
            new MovieGenre { MovieID = 1, GenreID = 3 },
            new MovieGenre { MovieID = 2, GenreID = 3 },
            new MovieGenre { MovieID = 3, GenreID = 1 },
            new MovieGenre { MovieID = 3, GenreID = 3 },
            new MovieGenre { MovieID = 4, GenreID = 2 },
            new MovieGenre { MovieID = 4, GenreID = 3 },
            new MovieGenre { MovieID = 5, GenreID = 4 }
        );

        // Show Time
        builder.Entity<Showtime>().HasData(
            new Showtime { ID = 1, MovieID = 1, StartTime = DateTime.Parse("2025-11-05T18:30:00"), TheaterID = 1 },
            new Showtime { ID = 2, MovieID = 1, StartTime = DateTime.Parse("2025-11-05T21:00:00"), TheaterID = 2 },
            new Showtime { ID = 3, MovieID = 2, StartTime = DateTime.Parse("2025-11-06T19:00:00"), TheaterID = 1 },
            new Showtime { ID = 4, MovieID = 3, StartTime = DateTime.Parse("2025-11-06T17:30:00"), TheaterID = 3 }
        );

        // Theatre
        builder.Entity<Theater>().HasData(
            new Theater { ID = 1, Name = "No 1", Capacity = 30 },
            new Theater { ID = 2, Name = "No 2", Capacity = 40 },
            new Theater { ID = 3, Name = "No 3", Capacity = 50 }
        );




    }
}
