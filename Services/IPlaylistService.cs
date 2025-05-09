using MoviesApp.DTOs;

namespace MoviesApp.Services
{
    public interface IPlaylistService
    {
        Task<PlaylistDto> Get(int id);
        Task<IEnumerable<PlaylistDto>> GetAll();
        Task<IEnumerable<PlaylistDto>> GetAllByUser(string user);
        Task<PlaylistDto> Add(PlaylistDto playlist);
        Task<PlaylistDto> Update(PlaylistDto playlist);
        Task<bool> Delete(int id);

    }
}
