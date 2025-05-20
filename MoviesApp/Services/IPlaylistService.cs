using MoviesApp.DTOs;

namespace MoviesApp.Services
{
    public interface IPlaylistService
    {
        Task<PlaylistDto> GetById(int id);
        Task<PlaylistDto> GetByIdNoTracking(int id);
        Task<IEnumerable<PlaylistDto>> GetAll();
        Task<IEnumerable<PlaylistDto>> GetAllByUser(string user);

        Task<PlaylistDto> Add(PlaylistDto movie);
        Task<PlaylistDto> Update(PlaylistDto movie);
        Task<bool> Delete(int id);
    }
}
