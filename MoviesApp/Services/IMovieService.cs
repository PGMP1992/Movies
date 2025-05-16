using MoviesApp.DTOs;

namespace MoviesApp.Services
{
    public interface IMovieService
    {
        Task<MovieDto> Get(int id);
        Task<IEnumerable<MovieDto>> GetAll();
        Task<IEnumerable<MovieDto>> GetAllActive();
        Task<IEnumerable<MovieDto>> GetByName(string name);

        Task<MovieDto> Add(MovieDto movie);
        Task<MovieDto> Update(MovieDto movie);
        Task<bool> Delete(int id);
    }
}
