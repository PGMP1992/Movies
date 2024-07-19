using MoviesApp.Models;

namespace MoviesApp.Repos.Interfaces
{
    public interface IPlaylistRepos
    {
        Task<List<Playlist>> GetAll();
        Task<Playlist> GetByIdAsync(int? id);
        Playlist GetPlaylistById(int id);
        Task<List<Playlist>> GetByName(string name);
        Task<List<Playlist>> GetAllByUserName(string userName);

        bool Add(Playlist playlist);
        bool Update(Playlist playlist);
        bool Delete(Playlist playlist);
        bool Save();
        bool PlaylistExists(int id);

    }
}
