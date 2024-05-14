using MoviesApp.Models;

namespace MoviesApp.Interfaces
{
    public interface IDashboardRepos
    {
        //Task<List<Movie>> GetAllUserMovies();
        Task<List<Playlist>> GetAllUserPlaylists();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
