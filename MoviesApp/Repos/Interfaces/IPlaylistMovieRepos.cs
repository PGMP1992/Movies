using MoviesApp.Models;

namespace MoviesApp.Repos.Interfaces
{
    public interface IPlaylistMovieRepos
    {
        Task<List<PlaylistMovie>> GetAll();
        Task<PlaylistMovie> GetById(int? id);
        Task<PlaylistMovie> GetByIdNoTracking(int? id);
        Task<List<PlaylistMovie>> GetAllByUser(string userName);

        bool Add(PlaylistMovie obj);
        bool Update(PlaylistMovie obj);
        bool Delete(PlaylistMovie obj);
        bool Save();
        bool Exists(int id);
    }
}
