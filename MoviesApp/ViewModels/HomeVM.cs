using MoviesApp.Models;

namespace MoviesApp.ViewModels
{
    public class HomeVM
    {
        public List<AppUser>? Users { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
