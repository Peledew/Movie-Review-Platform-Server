using Microsoft.EntityFrameworkCore;
using MovieReviewerPlatform.Domain.Entities;


namespace MovieReviewerPlatform.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<UserComment> UserComments { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("users");
            builder.Entity<Movie>().ToTable("movies");
            builder.Entity<UserComment>().ToTable("userComments");
            builder.Entity<UserRating>().ToTable("userRatings");
            builder.Entity<Genre>().ToTable("genres");

            // Configure cascading delete for Movie -> UserRating
            builder.Entity<Movie>()
                .HasMany(m => m.UserRatings)
                .WithOne(ur => ur.Movie)
                .HasForeignKey(ur => ur.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure cascading delete for Movie -> UserComment
            builder.Entity<Movie>()
                .HasMany(m => m.UserComments)
                .WithOne(uc => uc.Movie)
                .HasForeignKey(uc => uc.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure cascading delete for User -> UserRating
            builder.Entity<UserRating>()
                .HasOne(ur => ur.User)
                .WithMany() // No need for a collection in User
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure cascading delete for User -> UserComment
            builder.Entity<UserComment>()
                .HasOne(uc => uc.User)
                .WithMany() // No need for a collection in User
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
