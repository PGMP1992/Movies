using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.Models;
using MoviesApp.Helpers;
using MoviesApp.Services;
using MoviesApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Using External Services ================================================
builder.Services.AddScoped<IPhotoService, PhotoService>(); // Cloudinary Interface
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

//API Services using WebExecuter
builder.Services.AddHttpClient("MoviesApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseServerUrl"]); // API URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddHttpClient("AuthApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthoUrl"]); // API URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddTransient<IWebApiExecutor, WebApiExecutor>();

builder.Services.AddSession( options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// DB & Identity =========================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Has to be before authorization. 
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//creating Roles - OK
using (var scope = app.Services.CreateScope())
{
    var roleManager =
            scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "admin", "user" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

// creating Roles with the same password
using (var scope = app.Services.CreateScope())
{
    var userManager =
            scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    // Admin ---------------------------------
    string email = "admin@email.com";
    string password = "Admin123*";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new AppUser();
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "admin");
    }

    // User --------------------------------- 
    string userAcc = "user@email.com";

    if (await userManager.FindByEmailAsync(userAcc) == null)
    {
        var user1 = new AppUser();
        user1.UserName = userAcc;
        user1.Email = userAcc;
        user1.EmailConfirmed = true;

        await userManager.CreateAsync(user1, password);
        await userManager.AddToRoleAsync(user1, "user");
    }
}

app.Run();
