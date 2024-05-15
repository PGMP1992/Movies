using MoviesApp.Models;

namespace MoviesApp.Interfaces
{
    public interface XIDashboardRepos
    {
        Task<List<Playlist>> GetAllUserPlaylists();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
