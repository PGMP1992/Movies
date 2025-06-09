using Movies.Models;

namespace MoviesApp.Services.Interfaces
{
    public interface IPlaylistMovieService
    {
        Task<IEnumerable<PlaylistMovieDto>> GetAll();
        Task<IEnumerable<PlaylistMovieDto>> GetByPlaylist(int id);
        Task<IEnumerable<PlaylistMovieDto>> GetAllByUser(string user);
        Task<PlaylistMovieDto?> GetByContent(int playlistId, int movieId);

        Task<PlaylistMovieDto> GetById(int id);
        Task<PlaylistMovieDto> GetByIdNoTracking(int id);

        Task<PlaylistMovieDto> Add(PlaylistMovieDto obj);
        Task<PlaylistMovieDto> Update(PlaylistMovieDto obj);
        Task<bool> Delete(int id);

        Task<bool> MovieInPlaylist(int playlistId, int movieId);
    }
}

