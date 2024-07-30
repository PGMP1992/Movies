using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Models
{
    public class PlaylistMovie
    {
        public int Id { get; set; }

        public int PlaylistId { get; set; }
        [ForeignKey("PlaylistId")]
        [ValidateNever]
        public Playlist Playlist { get; set; }

        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        [ValidateNever]
        public Movie Movie { get; set; }
    }
}
