using MoviesApp.Models;

namespace MoviesApp.Repos.Interfaces
{
    public interface IMovieRepos
    {
        Task<IEnumerable<Movie>> GetAll();
        Task<IEnumerable<Movie>> GetAllActive();
        Task<Movie> GetById(int? id);
        Task<Movie> GetByIdNoTracking(int? id);
        Task<IEnumerable<Movie>> GetByName(string name);
        
        bool Add(Movie obj);
        bool Update(Movie obj);
        bool Delete(Movie obj);
        bool Save();
        bool Exists(int id);
    }
}
