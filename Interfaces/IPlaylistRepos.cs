using Microsoft.EntityFrameworkCore;
using MoviesApp.Models;

namespace MoviesApp.Interfaces
{
    public interface IPlaylistRepos
    {
        Task<List<Playlist>> GetAll();
        Task<Playlist> GetByIdAsync(int? id);
        Playlist GetPlaylistById(int id);
        Task<List<Playlist>> GetByName(string name);
        //Task<List<Playlist>> GetAllByUserName(string userName);
        //Task<List<Movie>> GetMovies(int? id);

        bool Add(Playlist playlist);
        bool Update(Playlist playlist);
        bool Delete(Playlist playlist);
        bool Save();
        bool PlaylistExists(int id);
        //bool AddMovieToPlaylist(Playlist playlist, Movie movie);
    }
}
