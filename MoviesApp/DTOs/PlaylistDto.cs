using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MoviesApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.DTOs
{
    public class PlaylistDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "";

        public string AppUserId { get; set; } = "";

        [ForeignKey("AppUserId")]
        [ValidateNever]
        public AppUser? AppUser { get; set; }

        [ValidateNever]
        public List<Movie>? Movies { get; set; }
    }
}
