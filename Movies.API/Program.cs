using Microsoft.EntityFrameworkCore;
using Movies.Business.Repos;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); 

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//Controllers
builder.Services.AddTransient<IUserRepos, UserRepos>();
builder.Services.AddTransient<IMovieRepos, MovieRepos>();
builder.Services.AddTransient<IPlaylistRepos, PlaylistRepos>();
builder.Services.AddTransient<IPlaylistMovieRepos, PlaylistMovieRepos>();

//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        //options.Authority = builder.Configuration["AuthorityUrl"] ?? throw new InvalidOperationException("Authority URL not configured.");
//        options.Audience = "MoviesAPI";
//        options.RequireHttpsMetadata = false; // Set to true in production
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


await using (var serviceScope = app.Services.CreateAsyncScope())
await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
