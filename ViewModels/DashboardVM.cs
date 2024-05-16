using MoviesApp.Models;

namespace MoviesApp.ViewModels
{
    public class DashboardVM
    {
        //public List<Movie> Movies { get; set; } 
        public string City { get; set; }
        public string State { get; set; }
        public string? ProfileImageUrl { get; set; }
        public List<Playlist> Playlists { get; set; }
    }
}
