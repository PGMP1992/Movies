﻿using MoviesApp.Models;

namespace MoviesApp.Interfaces
{
    public interface IMovieRepos
    {
        Task<List<Movie>> GetAll();
        Task<Movie> GetByIdAsync( int? id);
        Task<List<Movie>> GetByName( string name);
        Task<List<Movie>> GetByGenre( string genre);
        Task<List<Movie>> GetByAge( int age);

        bool Add( Movie movie );
        bool Update( Movie movie );
        bool Delete( Movie movie );
        bool Save();
        bool MovieExists(int id);
    }
}
