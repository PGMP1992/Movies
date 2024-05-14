using System.Collections;

namespace MoviesApp.ViewModels
{
    public class HomeVM
    {
        public IEnumerable  Playlists { get; set; }
        public string City { get; set;}
        public string State { get; set;}
    }
}
