using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Models
{
    public class Movie
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
        
        [Required]
        [MaxLength(500)]
        [Display(Name = "Picture URL")]
        public string PictUrl { get; set; } = "";

        [Required]
        [Display(Name = "Buy")]
        [DataType(DataType.Currency)]
        public double BuyPrice { get; set; }

        [Required]
        [Display(Name = "Rent")]
        [DataType(DataType.Currency)]
        public double RentPrice { get; set; }

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; } 
    }
}
