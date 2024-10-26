using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

//Created this for Saving Picture to Cloudinary

namespace MoviesApp.Models.ViewModels
{
    public class MovieVM
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
        [Range(1, 18)]
        [Display(Name = "Minimun Age")]
        public int Age { get; set; }

        public string? PictUrl { get; set; } = "";

        [ValidateNever]
        [MaxLength(300)]
        [Display(Name = "Picture URL")]
        public IFormFile Image { get; set; }
    }
}

