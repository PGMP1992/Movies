using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MoviesApp.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Models
{
    public class Playlist
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "";

        public string AppUserId { get; set; } = "";
        
        [ForeignKey("AppUserId")]
        [ValidateNever]
        public AppUser? AppUser { get; set; }

        //[ValidateNever]
        //public List<Movie>? Movies { get; set; }

        public PlaylistDto ToDto()
        {
            return new PlaylistDto
            {
                Id = this.Id,
                Name = this.Name,
                AppUserId = this.AppUserId,
                AppUser = this.AppUser,
                //Movies = this.Movies
            };
        }
    }
}
