using Microsoft.AspNetCore.Http;

namespace Movies.DataAccess.ViewModels
{
    public class EditUserVM
    {
        public string Id { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ProfileImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
