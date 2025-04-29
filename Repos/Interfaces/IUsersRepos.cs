using MoviesApp.Models;

namespace MoviesApp.Repos.Interfaces
{
    public interface IUsersRepos
    {
        Task<List<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);
        Task<List<Playlist>> GetAllUserPlaylists(string id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
