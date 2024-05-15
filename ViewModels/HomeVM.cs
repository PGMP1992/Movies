using MoviesApp.Models;
using System.Collections;

namespace MoviesApp.ViewModels
{
    public class HomeVM
    {
        public IEnumerable <Playlist> Playlists { get; set; }
        public string City { get; set;}
        public string State { get; set;}
    }
}
