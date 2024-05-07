using Microsoft.AspNetCore.Identity;

namespace MoviesApp.Models
{
    public class AppUser : IdentityUser
    {
        public List<Movie> Movies { get; set; }
        public List<Playlist> Playlists { get; set; }
        public string? ProfileImageryUrl { get; set; }
    }
}
