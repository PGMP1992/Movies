namespace Movies.DataAccess.ViewModels
{
    public class AddMovieVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Genre { get; set; } = "";
        public int Age { get; set; }
        public string PictUrl { get; set; } = "";
        public int PlaylistId { get; set; }
        public bool Active { get; set; }
    }
}
