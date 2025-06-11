using Movies.DataAccess.Models;
using Movies.Models;

namespace Movies.DataAccess.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<AppUserDto>? Users { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
