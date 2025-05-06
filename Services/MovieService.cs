using Microsoft.EntityFrameworkCore;
using Movies.DataSource.Data;
using MoviesApp.DTOs;

namespace MovieAPI.Services
{
    public class MovieService : IMovieService
    {
        //private readonly ApplicationDbContext _dbContext;
        //private readonly ILogger<MovieService> _logger;

        //public MovieService(ApplicationDbContext dbContext, ILogger<MovieService> logger)
        //{
        //    _dbContext = dbContext;
        //    _logger = logger;
        //}


        //public async Task<MovieDTO> Create(CreateMovieDto command)
        //{
        //    var movie = Movie.Create(command.Title, command.Genre, command.ReleaseDate, command.Rating);

        //    await _dbContext.Movies.AddAsync(movie);
        //    await _dbContext.SaveChangesAsync();

        //    return new MovieDTO(
        //       movie.Id,
        //       movie.Title,
        //       movie.Genre,
        //       movie.ReleaseDate,
        //       movie.Rating
        //    );
        //}

        //public async Task<IEnumerable<MovieDTO>> GetAll()
        //{
        //    return await _dbContext.Movies
        //        .AsNoTracking()
        //        .Select(movie => new MovieDTO(
        //            movie.Id,
        //            movie.Title,
        //            movie.Genre,
        //            movie.ReleaseDate,
        //            movie.Rating
        //        ))
        //        .ToListAsync();
        //}

        ////public async Task<MovieDto?> GetById(Guid id)
        //public async Task<MovieDTO?> GetById(int id)
        //{
        //    var movie = await _dbContext.Movies
        //                           .AsNoTracking()
        //                           .FirstOrDefaultAsync(m => m.Id == id);
        //    if (movie == null)
        //        return null;

        //    return new MovieDTO(
        //        movie.Id,
        //        movie.Title,
        //        movie.Genre,
        //        movie.ReleaseDate,
        //        movie.Rating
        //    );
        //}

        ////public async Task Update(Guid id, UpdateMovieDto command)
        //public async Task Update(int id, UpdateMovieDto command)
        //{
        //    var movieToUpdate = await _dbContext.Movies.FindAsync(id);
        //    if (movieToUpdate is null)
        //        throw new ArgumentNullException($"Invalid Movie Id.");

        //    movieToUpdate.Update(command.Title, command.Genre, command.ReleaseDate, command.Rating);
        //    await _dbContext.SaveChangesAsync();
        //}

        ////public async Task Delete(Guid id)
        //public async Task Delete(int id)
        //{
        //    var movieToDelete = await _dbContext.Movies.FindAsync(id);
        //    if (movieToDelete != null)
        //    {
        //        _dbContext.Movies.Remove(movieToDelete);
        //        await _dbContext.SaveChangesAsync();
        //    }
        //}
    }
}
