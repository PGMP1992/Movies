using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Models;
using MoviesApp.ViewModels;

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
        public DbSet<MoviesApp.ViewModels.UsersDetailsVM> UsersDetailsVM { get; set; } = default!;
    }
}
