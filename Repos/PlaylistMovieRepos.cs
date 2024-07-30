using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace MoviesApp.Repos
{
    public class PlaylistMovieRepos : IPlaylistMovieRepos
    {
        private readonly ApplicationDbContext _context;

        public PlaylistMovieRepos(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(PlaylistMovie playlistMovie)
        {
            _context.Add(playlistMovie);
            return Save();
        }

        public bool Delete(PlaylistMovie playlistMovie)
        {
            _context.Remove(playlistMovie);
            return Save();
        }

        public bool Update(PlaylistMovie playlistMovie)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<List<PlaylistMovie>> GetAll()
        {
            return await _context.PlaylistMovies
                .Include(p => p.Playlist)
                .Include(p => p.Movie)
                .ToListAsync();
        }

        public async Task<PlaylistMovie> GetByIdAsync(int? id)
        {
            return await _context.PlaylistMovies
               .Include(p => p.Playlist)
               .Include(p => p.Movie)
               .FirstOrDefaultAsync(p => p.Id == id);
        }

        public PlaylistMovie GetById(int id)
        {
            return _context.PlaylistMovies
                .Include(p => p.Playlist)
               .Include(p => p.Movie)
               .FirstOrDefault(m => m.Id == id);
        }

        public bool PlaylistMovieExists(int id)
        {
            return _context.PlaylistMovies.Any(e => e.Id == id);
        }
    }
}
