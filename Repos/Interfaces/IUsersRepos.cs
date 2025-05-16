using MoviesApp.Models;

namespace MoviesApp.Repos.Interfaces
{
    public interface IUsersRepos
    {
        Task<List<AppUser>> GetAll();
        Task<AppUser> GetById(string id);
        Task<List<Playlist>> GetAllPlaylists(string id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
