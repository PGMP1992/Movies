using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;
using MoviesApp.ViewModels;

namespace MoviesApp.Repos
{
    public class PlaylistMovieRepos : IPlaylistMovieRepos
    {
        private readonly ApplicationDbContext _context;

        public PlaylistMovieRepos(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlaylistMovie>> GetAll()
        {
            return await _context.PlaylistMovies
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PlaylistMovie> GetById(int? id)
        {
            return await _context.PlaylistMovies
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<PlaylistMovie> GetByIdNoTracking(int? id)
        {
            return await _context.PlaylistMovies
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<PlaylistMovie>> GetAllByUser(string appUser)
        {
            return await _context.PlaylistMovies
                .AsNoTracking()
                .Where(a => a.Playlist.AppUserId == appUser)
                //.Include(p => p.AppUser)
                //.Include(p => p.Movies)
                .ToListAsync();
        }

        public bool Add(PlaylistMovie obj)
        {
            _context.Add(obj);
            return Save();
        }

        public bool Delete(PlaylistMovie obj)
        {
            _context.Remove(obj);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(PlaylistMovie obj)
        {
            _context.Update(obj);
            return Save();
        }

        public bool Exists(int id)
        {
            return _context.PlaylistMovies
                .AsNoTracking()
                .Any(e => e.Id == id);
        }
               
    }
}
