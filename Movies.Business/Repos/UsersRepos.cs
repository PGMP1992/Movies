using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.Models;

namespace Movies.Business.Repos
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

        public async Task<List<AppUser>> GetAll()
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u=>u.UserName)
                .ToListAsync();
        }

        public async Task<AppUser> GetById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<Playlist>> GetAllPlaylists(string id)
        {
            //var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userPlaylists = _context.Playlists
                .AsNoTracking()
                .Where(r => r.AppUser.Id == id);
                //.Include(r => r.Movies);

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

        public bool Delete(AppUser user)
        {
            _context.Remove(user);
            return Save();
        }
    }
}
