using Microsoft.EntityFrameworkCore;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.Models;

namespace Movies.Business.Repos
{
    public class UserRepos(ApplicationDbContext _context) : IUserRepos
    {
        public async Task<List<AppUser>> GetAll()
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }

        public async Task<AppUser?> GetById(string id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<AppUser?> GetByIdNoTracking(string id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Playlist?>> GetAllPlaylists(string id)
        {
            return await _context.Playlists
                .AsNoTracking()
                //.Where(x => x.AppUser.Id == id)
                .Where(x => x.AppUser != null && x.AppUser.Id == id)
                .OrderBy(x => x.Name)
                .ToListAsync();
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
