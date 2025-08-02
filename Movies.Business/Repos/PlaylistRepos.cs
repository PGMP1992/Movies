using Microsoft.EntityFrameworkCore;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.Models;

namespace Movies.Business.Repos
{
    public class PlaylistRepos(ApplicationDbContext _context) : IPlaylistRepos
    { 
        public async Task<List<Playlist>> GetAll()
        {
            return await _context.Playlists
                .AsNoTracking()
                .Include(p => p.AppUser)
                .ToListAsync();
        }

        public async Task<List<Playlist>> GetAllByUser(string appUser)
        {
            return await _context.Playlists
                .AsNoTracking()
                .Where(a => a.AppUserId == appUser)
                .Include(p => p.AppUser)
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<Playlist?> GetById(int? id)
        {
            return await _context.Playlists
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Playlist?> GetByIdNoTracking(int? id)
        {
            return await _context.Playlists
                .AsNoTracking()
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Playlist>> GetByName(string name)
        {
            return await _context.Playlists
                .AsNoTracking()
                .Where(n => n.Name == name)
                .Include(u => u.AppUser)
                .OrderBy(n => n.Name)
                .ToListAsync();
        }

        public bool Add(Playlist obj)
        {
            _context.Add(obj);
            return Save();
        }

        public bool Delete(Playlist obj)
        {
            _context.Remove(obj);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Playlist obj)
        {
            _context.Update(obj);
            return Save();
        }
    }
}
