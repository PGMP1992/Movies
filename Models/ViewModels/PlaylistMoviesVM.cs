﻿using MoviesApp.Models;

namespace MoviesApp.Models.ViewModels
{
    public class PlaylistMoviesVM
    {
        public int? PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public List<Movie> MovieList { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
