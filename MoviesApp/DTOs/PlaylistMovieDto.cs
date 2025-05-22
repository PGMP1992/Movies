using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.DTOs
{
    public class PlaylistMovieDto
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }

        [ForeignKey("PlaylistId")]
        public PlaylistDto? Playlist { get; set; }
        
        public int MovieId { get; set; }
        
        [ForeignKey("MovieId")]
        public MovieDto? Movie { get; set; }
    }
}
