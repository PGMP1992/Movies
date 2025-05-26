using Microsoft.EntityFrameworkCore;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.Models;

namespace Movies.Business.Repos
{
    public class PlaylistMovieRepos : IPlaylistMovieRepos
    {
        private readonly ApplicationDbContext _context;

        public PlaylistMovieRepos(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlaylistMovie>> GetAll()
        {
            return await _context.PlaylistMovies
                .AsNoTracking()
                .Include(p => p.Playlist)
                .Include(p => p.Movie)
                .OrderBy(p => p.Movie.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlaylistMovie>> GetAllByUser(string appUser)
        {
            return await _context.PlaylistMovies
                .AsNoTracking()
                .Where(a => a.Playlist.AppUserId == appUser)
                .Include(p => p.Playlist)
                .Include(p => p.Movie)
                        .OrderBy(p => p.Movie.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlaylistMovie>> GetByPlaylist(int id)
        {
            return await _context.PlaylistMovies
                .AsNoTracking()
                .Where(a => a.PlaylistId == id)
                .Include(p => p.Playlist)
                .Include(p => p.Movie)
                .OrderBy(p => p.Movie.Title)
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

        public bool MovieInPlaylist(int playlistId, int movieId)
        {
            var result = _context.PlaylistMovies
                .Where(a => a.PlaylistId == playlistId && a.MovieId == movieId);
            return result.Any();
        }

        public async Task<PlaylistMovie> GetByContent(int playlistId, int movieId)
        {
            return await _context.PlaylistMovies
                .FirstOrDefaultAsync(a => a.PlaylistId == playlistId && a.MovieId == movieId);
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
