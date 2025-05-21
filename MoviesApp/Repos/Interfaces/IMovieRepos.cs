using MoviesApp.Models;

namespace MoviesApp.Repos.Interfaces
{
    public interface IMovieRepos
    {
        Task<List<Movie>> GetAll();
        Task<List<Movie>> GetAllActive();
        Task<Movie> GetById(int? id);
        Task<Movie> GetByIdNoTracking(int? id);
        Task<List<Movie>> GetByName(string name);
        
        bool Add(Movie obj);
        bool Update(Movie obj);
        bool Delete(Movie obj);
        bool Save();
        bool Exists(int id);
    }
}
