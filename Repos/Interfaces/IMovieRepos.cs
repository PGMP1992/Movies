using MoviesApp.Models;

namespace MoviesApp.Repos.Interfaces
{
    public interface IMovieRepos
    {
        Task<List<Movie>> GetAll(bool active);
        Task<Movie> GetById(int? id);
        Task<Movie> GetByIdNoTracking(int? id);
        Task<List<Movie>> GetByName(string name);
        
        bool Add(Movie movie);
        bool Update(Movie movie);
        bool Delete(Movie movie);
        bool Save();
        bool MovieExists(int id);
    }
}
