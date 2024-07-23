using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
    public class EditProfileVM
    {
        public string? City { get; set; }
        public string? State { get; set; }
        [Display(Name = "Picture URL")]
        public string? ProfileImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
