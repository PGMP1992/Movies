using MoviesApp.Models;

namespace MoviesApp.Repos.Interfaces
{
    public interface IPlaylistMovieRepos
    {
        Task<List<PlaylistMovie>> GetAll();
        Task<List<PlaylistMovie>> GetByPlaylist(int? id);
        Task<List<PlaylistMovie>> GetAllByUser(string userName);
        Task<PlaylistMovie> GetByContent(int playlistId, int movieId);

        Task<PlaylistMovie> GetById(int? id);
        Task<PlaylistMovie> GetByIdNoTracking(int? id);
                
        bool Add(PlaylistMovie obj);
        bool Update(PlaylistMovie obj);
        bool Delete(PlaylistMovie obj);
        
        bool Save();
        bool Exists(int id);
        bool MovieInPlaylist(PlaylistMovie obj);
    }
}
