using MoviesApp.DTOs;

namespace MoviesApp.Services
{
    public interface IPlaylistMovieService
    {
        Task<PlaylistMovieDto> GetById(int id);
        Task<PlaylistMovieDto> GetByIdNoTracking(int id);
        Task<IEnumerable<PlaylistMovieDto>> GetAll();
        Task<IEnumerable<PlaylistMovieDto>> GetAllByUser(string user);

        Task<PlaylistMovieDto> Add(PlaylistMovieDto obj);
        Task<PlaylistMovieDto> Update(PlaylistMovieDto obj);
        Task<bool> Delete(int id);
    }
}
