using Movies.DataAccess.Models;
using Movies.Models;

namespace Movies.DataAccess.ViewModels
{
    public class UsersDetailsVM
    {
        public string? Id { get; set; }  
        public string? UserName { get; set; }
        public string? City {  get; set; }
        public string? State { get; set; }
        public string? ProfileImageUrl { get; set; }
        public IEnumerable<PlaylistDto>? Playlists { get; set; }
    }
}
