namespace MoviesApp.Models.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ImageUrl { get; set; }
<<<<<<<< HEAD:Models/ViewModels/UserVM.cs
        public IEnumerable<Playlist> Playlists { get; set; }
========
>>>>>>>> ChangedModels:Models/ViewModels/EditUserVM.cs
        public IFormFile? Image { get; set; }
    }
}
