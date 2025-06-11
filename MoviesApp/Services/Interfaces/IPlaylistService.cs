using Movies.Models;

namespace MoviesApp.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<PlaylistDto> GetById(int id);
        Task<PlaylistDto> GetByIdNoTracking(int id);
        Task<IEnumerable<PlaylistDto>> GetAll();
        Task<IEnumerable<PlaylistDto>> GetAllByUser(string user);

        Task<PlaylistDto> Add(PlaylistDto obj);
        Task<PlaylistDto> Update(int id,PlaylistDto obj);
        Task<bool> Delete(int id);
    }
}
