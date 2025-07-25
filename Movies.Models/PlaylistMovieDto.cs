namespace Movies.Models
{
    public class PlaylistMovieDto
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }

        public PlaylistDto? Playlist { get; set; }
        
        public int MovieId { get; set; }
        
        public MovieDto? Movie { get; set; }
    }
}
