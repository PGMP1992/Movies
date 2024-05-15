namespace MoviesApp.ViewModels
{
    public class EditUserVM
    {
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ProfileImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
