using Movies.DataAccess.Models;

namespace Movies.DataAccess.ViewModels
{
    public class UsersDetailsVM
    {
        public string? Id { get; set; }  
        public string? UserName { get; set; }
        public string? City {  get; set; }
        public string? State { get; set; }
        public string? ProfileImageUrl { get; set; }
        public List<Playlist>? Playlists { get; set; }
    }
}
