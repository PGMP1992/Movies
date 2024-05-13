using MoviesApp.Models;

namespace MoviesApp.ViewModels
{
    public class UsersDetailsVM
    {
        public string Id { get; set; }  
        public string UserName { get; set; }
        public IFormFile Image { get; set; }
        public Address Address { get; set; }
    }
}
