using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace MoviesApp.Repos
{
    public class MovieRepos : IMovieRepos
    {
        private readonly ApplicationDbContext _context;
        private const int pageNumber = 1;
        private const int pageSize = 10;

        public MovieRepos(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetAll(bool active)
        {
            if(active)
            {
                return await _context.Movies
                    .AsNoTracking()
                    .Where(m => m.Active == true)
                    .OrderBy(m => m.Title)
                    .Include(m => m.Playlists)
                    .ToListAsync();
            }
            else
            {
                return await _context.Movies
                    .AsNoTracking()
                    .OrderBy(m => m.Title)
                    .Include(m => m.Playlists)
                    .ToListAsync();
            }
        }

        public async Task<Movie> GetById(int? id)
        {
            return await _context.Movies
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Movie> GetByIdNoTracking(int? id)
        {
            return await _context.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Movie>> GetByName(string name)
        {
            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Title!.Contains(name) && m.Active == true)
                .ToListAsync();
        }

        public bool Add(Movie movie)
        {
            _context.Add(movie);
            return Save();
        }

        public bool Delete(Movie movie)
        {
            _context.Remove(movie);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Movie movie)
        {
            movie.UpdateLastModified();
            _context.Update(movie);
            return Save();
        }

        public bool MovieExists(int id)
        {
            return _context.Movies
                .AsNoTracking()
                .Any(e => e.Id == id);
        }
    }
}
