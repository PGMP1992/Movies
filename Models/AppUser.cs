using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Models
{
    public class AppUser : IdentityUser
    {
        public string? ProfileImageryUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        
        public List<Playlist> Playlists { get; set; }
    }
}
