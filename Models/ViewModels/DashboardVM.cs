using MoviesApp.Models;

namespace MoviesApp.Models.ViewModels
{
    public class DashboardVM
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string City { get; set; }
        public string State { get; set; }
        public string? ImageUrl { get; set; }
        public List<Playlist> Playlists { get; set; }
    }
}
