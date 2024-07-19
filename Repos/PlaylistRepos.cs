using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Interfaces;
using MoviesApp.Models;

namespace MoviesApp.Repos
{
    public class PlaylistRepos : IPlaylistRepos
    { 
        private readonly ApplicationDbContext _context;

        public PlaylistRepos(ApplicationDbContext context)
        {
            _context = context;
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
        
        public async Task<List<Playlist>> GetAll()
        {
            return await _context.Playlists
                .Include(p=>p.AppUser)
                .ToListAsync();
        }

        public async Task<Playlist> GetByIdAsync(int? id)
        {
            return await _context.Playlists
               //.Include(m => m.MoviesList)
               .FirstOrDefaultAsync(m => m.Id == id);
        }

        public Playlist GetPlaylistById(int id)
        {
            return _context.Playlists
               .Include(m => m.MoviesList)
               .FirstOrDefault(m => m.Id == id);
        }

        public async Task<List<Playlist>> GetByName(string name)
        {
            return await _context.Playlists.Where(n => n.Name == name).ToListAsync();
        }

        public bool PlaylistExists(int id)
        {
            return _context.Playlists.Any(e => e.Id == id);
        }

        public async Task<List<Playlist>> GetAllByUserName(string appUser)
        {
            return await _context.Playlists.Where(a => a.AppUserId == appUser).ToListAsync();
        }

        public bool AddMovie(Movie movie)
        {
            
            return Save();
        }
    }
}
