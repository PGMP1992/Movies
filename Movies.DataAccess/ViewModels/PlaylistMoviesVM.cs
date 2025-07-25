using Movies.Models;

namespace Movies.DataAccess.ViewModels
{
    public class PlaylistMoviesVM
    {
        public PlaylistDto? Playlist {  get; set; } 
        public IEnumerable<PlaylistMovieDto>? Playlists { get; set; }
    }
}
