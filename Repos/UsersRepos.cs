using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace MoviesApp.Repos
{
    public class UsersRepos : IUsersRepos
    {
        private readonly ApplicationDbContext _context;

        public UsersRepos(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Delete(AppUser user)
        {
            _context.Remove(user);
            return Save();
        }

        public async Task<List<AppUser>> GetAll()
        {
            return await _context.AppUsers.OrderBy(u=>u.UserName).ToListAsync();
        }

        public async Task<AppUser> GetById(string id)
        {
            return await _context.AppUsers.FindAsync(id);
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
