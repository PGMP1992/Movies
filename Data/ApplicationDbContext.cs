using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MoviesApp.Models;

namespace MoviesApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // // Added to enable Identity - Not sure if that is a bug 
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Playlist>()
                .HasMany(e => e.MovieList)
                .WithMany(e => e.PlaylistList);

            modelBuilder.Entity<Movie>()
                .HasMany(e => e.PlaylistList)
                .WithMany(e => e.MovieList);
        }
    }

    
}
