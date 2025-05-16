using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace MoviesApp.Repos
{
    public class PlaylistRepos : IPlaylistRepos
    { 
        private readonly ApplicationDbContext _context;

        public PlaylistRepos(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Playlist>> GetAll()
        {
            return await _context.Playlists
                .AsNoTracking()
                .Include(p => p.AppUser)
                .Include(p => p.Movies)
                .ToListAsync();
        }

        public async Task<Playlist> GetById(int? id)
        {
            return await _context.Playlists
                .Include(p => p.AppUser)
                .Include(p => p.Movies)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Playlist> GetByIdNoTracking(int? id)
        {
            return _context.Playlists
                .AsNoTracking()
                .Include(p => p.AppUser)
                .Include(p => p.Movies)
                .FirstOrDefault(m => m.Id == id);
        }

        public async Task<List<Playlist>> GetByName(string name)
        {
            return await _context.Playlists
                .AsNoTracking()
                .Where(n => n.Name == name)
                .Include(p => p.AppUser)
                .Include(p => p.Movies)
                .ToListAsync();
        }

        public async Task<List<Playlist>> GetAllByUser(string appUser)
        {
            return await _context.Playlists
                .AsNoTracking()
                .Where(a => a.AppUserId == appUser)
                .Include(p => p.AppUser)
                .Include(p => p.Movies)
                .ToListAsync();
        }

        public bool Add(Playlist playlist)
        {
            _context.Add(playlist);
            return Save();
        }

        public bool Delete(Playlist playlist)
        {
            _context.Remove(playlist);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Playlist playlist)
        {
            _context.Update(playlist);
            return Save();
        }

        public bool PlaylistExists(int id)
        {
            return _context.Playlists
                .AsNoTracking()
                .Any(e => e.Id == id);
        }
    }
}
