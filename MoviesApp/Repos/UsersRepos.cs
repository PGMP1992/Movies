using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace MoviesApp.Repos
{
    public class UsersRepos : IUsersRepos
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersRepos(ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
            _context.Remove(user);
            return Save();
        }

        public async Task<List<AppUser>> GetAll()
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u=>u.UserName)
                .ToListAsync();
        }

        public async Task<AppUser> GetById(string id)
        {
            return await _context.Users
                .FindAsync(id);
        }

        public async Task<List<Playlist>> GetAllPlaylists(string id)
        {
            //var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userPlaylists = _context.Playlists
                .AsNoTracking()
                .Where(r => r.AppUser.Id == id)
                .Include(r => r.Movies);

            return userPlaylists.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
