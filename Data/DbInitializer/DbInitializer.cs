using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;

namespace Data.DbInitializer;

public class DbInitializer : IDbInitializer
{
    // For some strange reason I had to change the line below to use AppUser instead of IdentityUser - PM
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _db;

    public DbInitializer(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
    }

    public void Initialize()
    {
        try
        {
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
        }

        _db.Database.EnsureCreated();

        if (!_roleManager.RoleExistsAsync("Customer").GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole("admin")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("user")).GetAwaiter().GetResult();
        }

        // Create Admin User
        _userManager.CreateAsync(new AppUser()
        {
            UserName = "admin@host.com",
            Email = "admin@host.com",
            
        }, "Admin123*").GetAwaiter().GetResult();

        AppUser user = _db.AppUsers.FirstOrDefault(u => u.Email == "admin@host.com");
        _userManager.AddToRoleAsync(user, "admin").GetAwaiter().GetResult();

        // Create User
        _userManager.CreateAsync(new AppUser()
        {
            UserName = "user@host.com",
            Email = "user@host.com",

        }, "Admin123*").GetAwaiter().GetResult();

        AppUser user1 = _db.AppUsers.FirstOrDefault(u => u.Email == "user@host.com");
        _userManager.AddToRoleAsync(user1, "user").GetAwaiter().GetResult();

        return;
    }
}
