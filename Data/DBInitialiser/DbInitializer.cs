using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Models;
using MoviesApp.Utility;

namespace MoviesApp.Data.DBInitialiser
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            CreateUsersRoles();
            SeedData();
        }

        public void CreateUsersRoles()
        {
            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();

                //if roles are not created, then we will create Admin and Costumer user as well
                _userManager.CreateAsync(new AppUser
                {
                    UserName = "admin@host.com",
                    Email = "admin@host.com",
                    PhoneNumber = "111111111",
                    City = "City",
                    State = "State",
                }, "Admin123*").GetAwaiter().GetResult();

                AppUser user = _db.AppUsers.FirstOrDefault(u => u.Email == "admin@host.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

                _userManager.CreateAsync(new AppUser
                {
                    UserName = "user@host.com",
                    Email = "user@host.com",
                    PhoneNumber = "2222222222",
                    City = "City",
                    State = "State",
                }, "Admin123*").GetAwaiter().GetResult();

                user = _db.AppUsers.FirstOrDefault(u => u.Email == "user@host.com");
                _userManager.AddToRoleAsync(user, SD.Role_User).GetAwaiter().GetResult();
            }
            return;
        }

        public void SeedData()
        {

        }

    }
}
