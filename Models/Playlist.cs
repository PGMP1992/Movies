﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        [ValidateNever]
        [MaxLength(100)]
        public string Description { get; set; } = "";

        public string AppUserId { get; set; } = "";
        
        [ValidateNever]
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        [ValidateNever]
        public List<Movie> MoviesList { get; set; }
    }
}
