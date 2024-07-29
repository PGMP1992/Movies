using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
    public class EditMovieVM
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = "";

        [Required]
        [MaxLength(300)]
        public string Description { get; set; } = "";

        [Required]
        [MaxLength(30)]
        public string Genre { get; set; } = "";

        [Required]
        [Range(0, 18)]
        [Display(Name = "Minimun Age")]
        public int Age { get; set; }

        [Display(Name = "Picture URL")]
        public string? PictUrl { get; set; }

        public IFormFile? Image { get; set; }
    }
}

