using MoviesApp.Models;

namespace MoviesApp.Repos.Interfaces
{
    public interface IPlaylistMovieRepos
    {
        Task<List<PlaylistMovie>> GetAll();
        Task<PlaylistMovie> GetById(int? id);
        //PlaylistMovie GetById(int id);

        bool Add(PlaylistMovie playlistMovie);
        bool Update(PlaylistMovie playlistMovie);
        bool Delete(PlaylistMovie playlistMovie);
        bool Save();
        bool PlaylistMovieExists(int id);
    }
}
