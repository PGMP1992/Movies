using Microsoft.AspNetCore.Identity;
using Movies.Models;

namespace Movies.DataAccess.Models
{
    public class AppUser : IdentityUser
    {
        public string? ProfileImageryUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public List<Playlist>? Playlists { get; set; }
        
        public AppUserDto ToDto()
        {
            return new AppUserDto
            {
                Id = this.Id,
                City = this.City,
                State = this.State,
                ProfileImageryUrl = this.ProfileImageryUrl,
                UserName = this.UserName,
                Email = this.Email,
            };
        }
    }
}
