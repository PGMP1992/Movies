namespace MoviesApp.Models.ViewModels
{
    public class EditUserVM
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
