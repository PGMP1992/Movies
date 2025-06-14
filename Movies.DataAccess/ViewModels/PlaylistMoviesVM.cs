﻿using Movies.Models;

namespace Movies.DataAccess.ViewModels
{
    public class PlaylistMoviesVM
    {
        public PlaylistDto? Playlist {  get; set; } 
        public IEnumerable<PlaylistMovieDto>? Playlists { get; set; }
        //public List<MovieDto>? Movies { get; set;}
        //public string? AppUserId { get; set; }
        //public AppUser? AppUser { get; set; }
    }
}
