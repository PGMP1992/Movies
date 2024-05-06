using MoviesApp.Models;

namespace MoviesApp.ViewModels
{
    public class DashboardVM
    {
        public List<Movie> Movies { get; set; } 
        public List<Playlist> Playlists { get; set; }
    }
}
