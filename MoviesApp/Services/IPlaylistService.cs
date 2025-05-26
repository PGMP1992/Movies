using Movies.Models;

namespace MoviesApp.Services
{
    public interface IPlaylistService
    {
        Task<PlaylistDto> GetById(int id);
        Task<PlaylistDto> GetByIdNoTracking(int id);
        Task<IEnumerable<PlaylistDto>> GetAll();
        Task<IEnumerable<PlaylistDto>> GetAllByUser(string user);

        Task<PlaylistDto> Add(PlaylistDto obj);
        Task<PlaylistDto> Update(PlaylistDto obj);
        Task<bool> Delete(int id);
    }
}
