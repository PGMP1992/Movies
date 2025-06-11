using Movies.Models;

namespace MoviesApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<AppUserDto>> GetAll();
        Task<AppUserDto> GetById(string id);
        Task<AppUserDto> GetByIdNoTracking(string id);
        Task<IEnumerable<PlaylistDto>> GetAllPlaylists(string id);
        Task<AppUserDto> Update(string id, AppUserDto user);
        Task<bool> Delete(string id);
    }
}