using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "";

        [ValidateNever]
        [MaxLength(100)]
        public string Description { get; set; } = "";
        
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; } = "";
        public AppUser? AppUser { get; set; }

        [ValidateNever]
        //[Required]
        bool IsRental { get; set; } = false;

        //[ValidateNever]
        //public List<Movie> MoviesList { get; set; }

    }
}
