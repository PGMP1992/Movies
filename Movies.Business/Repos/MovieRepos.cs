using Microsoft.EntityFrameworkCore;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.Models;

namespace Movies.Business.Repos
{
    public class MovieRepos(ApplicationDbContext _context) : IMovieRepos
    {
        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await _context.Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllActive()
        {
            return await _context.Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .Where(m => m.Active == true)
                .ToListAsync();
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

        public async Task<IEnumerable<Movie>> GetByName(string name)
        {
            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Title!.Contains(name) && m.Active == true)
                .ToListAsync();
        }

        public bool Add(Movie obj)
        {
            _context.Add(obj);
            return Save();
        }

        public bool Delete(Movie obj)
        {
            _context.Remove(obj);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Movie obj)
        {
            _context.Update(obj);
            return Save();
        }
               
    }
}
