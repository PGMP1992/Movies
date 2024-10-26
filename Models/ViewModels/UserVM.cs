namespace MoviesApp.Models.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ImageUrl { get; set; }
        public List<Playlist> playlists { get; set; }
    }
}
