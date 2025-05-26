using Movies.DataAccess.Models;

namespace Movies.Business.Repos.Interfaces
{
    public interface IPlaylistRepos
    {
        Task<List<Playlist>> GetAll();
        Task<Playlist> GetById(int? id);
        Task<Playlist> GetByIdNoTracking(int? id);
        Task<List<Playlist>> GetByName(string name);
        Task<List<Playlist>> GetAllByUser(string userName);

        bool Add(Playlist obj);
        bool Update(Playlist obj);
        bool Delete(Playlist obj);
        bool Save();
        bool Exists(int id);

    }
}
