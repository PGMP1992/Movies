using Microsoft.AspNetCore.Identity;
using Movies.Models;

namespace Movies.DataAccess.Models
{
    public class AppUser : IdentityUser
    {
        public string UserName { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? ProfileImageryUrl { get; set; } = string.Empty;

        //public List<Playlist>? Playlists { get; set; }

        public AppUserDto ToDto()
        {
            return new AppUserDto
            {
                Id = this.Id,
                UserName = this.UserName,
                Email = this.Email,
                City = this.City,
                State = this.State,
                ProfileImageryUrl = this.ProfileImageryUrl,
            };
        }
    }
}
