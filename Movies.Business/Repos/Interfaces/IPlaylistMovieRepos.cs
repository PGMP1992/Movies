using Movies.DataAccess.Models;

namespace Movies.Business.Repos.Interfaces
{
    public interface IPlaylistMovieRepos
    {
        Task<IEnumerable<PlaylistMovie>> GetByPlaylist(int id);
        Task<PlaylistMovie> GetById(int? id);
                
        bool Add(PlaylistMovie obj);
        bool Delete(PlaylistMovie obj);
        bool Save();

        bool MovieInPlaylist(int playlistId, int movieId);
        Task<PlaylistMovie> GetByContent(int playlistId, int movieId);
    }
}
