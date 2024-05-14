﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MoviesApp.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
    public class EditMovieVM
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

        public string? PictUrl { get; set; }

        public int? PlaylistId { get; set; }
        public List<Playlist>? Playlists { get; set; }
    }
}

