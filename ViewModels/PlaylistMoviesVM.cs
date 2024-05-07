using MoviesApp.Models;

namespace MoviesApp.ViewModels
{
    public class PlaylistMoviesVM
    {
        public Playlist Playlist { get; set; }
        public List<Movie> Movies { get; set;}
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
