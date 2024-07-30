using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesApp.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
    public class AddMovieVM
    {
        public Playlist PlaylistId { get; set; }
        public int MovieId { get; set; }
        [ValidateNever]
        public Movie Movie { get; set; }
    }
}
