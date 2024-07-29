using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesApp.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
    public class AddMovieVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Genre { get; set; } = "";
        public int Age { get; set; }
        public string PictUrl { get; set; } = "";
        //public IEnumerable<SelectListItem> PlaylistSelect { get; set; }
        public int PlaylistId { get; set; }
    }
}
