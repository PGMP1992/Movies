﻿using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace MoviesApp.Repos
{
    public class DashboardRepos : IDashboardRepos
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepos( ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Playlist>> GetAllUserPlaylists()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userPlaylists = _context.Playlists.Where(r => r.AppUser.Id == curUser);
            return userPlaylists.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.AppUsers.FindAsync(id);
        }

        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.AppUsers.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }
        
    }

}
