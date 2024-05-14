using MoviesApp.Models;

namespace MoviesApp.ViewModels
{
    public class UsersDetailsVM
    {
        public string Id { get; set; }  
        public string UserName { get; set; }
        public IFormFile Image { get; set; }
        public string? PictUrl { get; set; } 
        public string City {  get; set; }
        public string State { get; set; }
        public List<Playlist> playlists { get; set; }
    }
}
