using MoviesApp.Models;

namespace MoviesApp.ViewModels
{
    public class DashboardVM
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string City { get; set; }
        public string State { get; set; }
        public string? ProfileImageUrl { get; set; }
        public List<Playlist> Playlists { get; set; }
    }
}
