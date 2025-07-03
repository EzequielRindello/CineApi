using CineApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Director> Directors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieFunction> MovieFunctions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure relations
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovieFunction>()
                .HasOne(mf => mf.Movie)
                .WithMany(m => m.MovieFunctions)
                .HasForeignKey(mf => mf.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // configure indexs
            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.Title);

            modelBuilder.Entity<MovieFunction>()
                .HasIndex(mf => mf.Date);

            base.OnModelCreating(modelBuilder);
        }
    }
}
