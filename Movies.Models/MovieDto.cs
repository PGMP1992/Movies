﻿using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class MovieDto
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

        public bool Active { get; set; }

        [MaxLength(500)]
        [Display(Name = "Picture URL")]
        public string? PictUrl { get; set; } = "";
        
        //public List<Playlist>? Playlists { get; set; }
    }
}
