using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movies.DataAccess.Models;

namespace Movies.DataAccess.Data
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

        // Many to many relationship between Playlist and Movies
        public DbSet<PlaylistMovie> PlaylistMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Added to enable Identity - Not sure if that is a bug 
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Playlist>()
            //    .HasMany(x => x.Movies)
            //    .WithMany(y => y.Playlists);

            //modelBuilder.Entity<Movie>()
            //    .HasMany(e => e.Playlists)
            //    .WithMany(e => e.Movies);
        }
    }


}
