using Movies.DataAccess.Models;

namespace Movies.DataAccess.ViewModels
{
    public class HomeVM
    {
        public List<AppUser>? Users { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
