using Movies.DataAccess.Models;

namespace Movies.Business.Repos.Interfaces
{
    public interface IPlaylistMovieRepos
    {
        Task<IEnumerable<PlaylistMovie>> GetAll();
        Task<IEnumerable<PlaylistMovie>> GetByPlaylist(int id);
        Task<IEnumerable<PlaylistMovie>> GetAllByUser(string userName);
        
        Task<PlaylistMovie> GetById(int? id);
        Task<PlaylistMovie> GetByIdNoTracking(int? id);
                
        bool Add(PlaylistMovie obj);
        bool Update(PlaylistMovie obj);
        bool Delete(PlaylistMovie obj);
        
        bool Save();
        bool Exists(int id);
        bool MovieInPlaylist(int playlistId, int movieId);
        Task<PlaylistMovie> GetByContent(int playlistId, int movieId);
    }
}
