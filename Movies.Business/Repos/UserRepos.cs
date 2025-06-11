using Microsoft.EntityFrameworkCore;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.Models;

namespace Movies.Business.Repos
{
    public class UserRepos : IUserRepos
    {
        private readonly ApplicationDbContext _context;

        public UserRepos(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppUser>> GetAll()
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }

        public async Task<AppUser> GetById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Playlist>> GetAllPlaylists(string id)
        {
            var userPlaylists = _context.Playlists
                .AsNoTracking()
                .Where(x => x.AppUser.Id == id)
                .OrderBy(x => x.Name);

            return userPlaylists.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            //var dbUser = _context.Users.Find(user.Id);
            //if (dbUser != null)
            //{
            //    dbUser.Id = user.Id;
            //    dbUser.UserName = user.UserName;
            //    dbUser.NormalizedUserName = user.NormalizedUserName;
            //    dbUser.Email = user.Email;
            //    dbUser.NormalizedEmail = user.NormalizedEmail;

            //    dbUser.City = user.City;
            //    dbUser.State = user.State;
            //    dbUser.ProfileImageryUrl = user.ProfileImageryUrl;

            //    dbUser.EmailConfirmed = user.EmailConfirmed;
            //    dbUser.PasswordHash = user.PasswordHash;
            //    dbUser.SecurityStamp = user.SecurityStamp;
            //    dbUser.ConcurrencyStamp = user.ConcurrencyStamp;
            //    dbUser.PhoneNumber = user.PhoneNumber;
            //    dbUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            //    dbUser.TwoFactorEnabled = user.TwoFactorEnabled;
            //    dbUser.LockoutEnd = user.LockoutEnd;
            //    dbUser.LockoutEnabled = user.LockoutEnabled;
            //    dbUser.AccessFailedCount = user.AccessFailedCount;
            //}
            _context.Users.Update(user);
            return Save();
        }

        public bool Delete(AppUser user)
        {
            _context.Remove(user);
            return Save();
        }

        public bool Exists(string id)
        {
            return _context.Users.Any(x => x.Id == id);
        }
    }
}
