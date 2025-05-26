using Movies.Models;

namespace MoviesApp.Services
{
    public interface IMovieService
    {
        Task<MovieDto> GetById(int id);
        Task<MovieDto> GetByIdNoTracking(int id);
        Task<IEnumerable<MovieDto>> GetAll();
        Task<IEnumerable<MovieDto>> GetAllActive();
        Task<IEnumerable<MovieDto>> GetByName(string name);

        Task<MovieDto> Add(MovieDto movie);
        Task<MovieDto> Update(MovieDto movie);
        Task<bool> Delete(int id);
    }
}
