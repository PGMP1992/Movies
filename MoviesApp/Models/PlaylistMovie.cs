using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MoviesApp.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Models
{
    public class PlaylistMovie
    {
        public int Id { get; set; }

        [Required]
        public int PlaylistId { get; set; }

        [ForeignKey("PlaylistId")]
        [ValidateNever]
        public Playlist? Playlist { get; set; }

        [Required]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        [ValidateNever]
        public Movie? Movie { get; set; }

        public PlaylistMovieDto ToDto()
        {
            return new PlaylistMovieDto
            {
                Id = this.Id,
                PlaylistId = this.PlaylistId,
                MovieId = this.MovieId
            };
        }
    }
}
