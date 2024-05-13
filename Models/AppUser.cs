using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Models
{
    public class AppUser : IdentityUser
    {
        //public List<Movie> Movies { get; set; }
        public string? ProfileImageryUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address Address { get; set; } 
        
        public List<Playlist> Playlists { get; set; }
    }
}
