using Movies.DataAccess.Models;

namespace Movies.Business.Repos.Interfaces
{
    public interface IUserRepos
    {
        Task<List<AppUser>> GetAll();
        Task<AppUser> GetById(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        Task<List<Playlist>> GetAllPlaylists(string id);
        
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
        bool Exists(string id);
    }
}
