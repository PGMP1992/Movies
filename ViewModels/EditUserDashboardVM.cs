namespace MoviesApp.ViewModels
{
    public class EditUserDashboardVM
    {
        public string Id { get; set; }
        public string? ProfileImageUrl {  get; set; } 
        public IFormFile Image {  get; set; }
    }
}
