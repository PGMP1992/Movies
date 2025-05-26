using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models
{
    public class PlaylistDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string AppUserId { get; set; }

        //[ForeignKey("AppUserId")]
        //[ValidateNever]
        public AppUserDto? AppUser { get; set; }

        //[ValidateNever]
        //public List<Movie>? Movies { get; set; }
    }
}
