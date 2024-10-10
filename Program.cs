using Microsoft.AspNetCore.Identity;
using Data.DbInitializer;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Data.DBInitialiser;
using MoviesApp.Helpers;
using MoviesApp.Models;
using MoviesApp.Repos;
using MoviesApp.Repos.Interfaces;
using MoviesApp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IMovieRepos, MovieRepos>();
builder.Services.AddScoped<IPlaylistRepos, PlaylistRepos>();
builder.Services.AddScoped<IDashboardRepos, DashboardRepos>();
builder.Services.AddScoped<IUsersRepos, UsersRepos>();
builder.Services.AddScoped<IPhotoService, PhotoService>(); // Cloudinary Interface
builder.Services.AddScoped<IDbInitializer, DbInitializer>(); // Feed Data
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
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
SeedDB();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDB()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
