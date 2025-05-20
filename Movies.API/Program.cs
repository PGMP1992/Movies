using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Repos;
using MoviesApp.Repos.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//Controllers
builder.Services.AddTransient<IMovieRepos, MovieRepos>();
builder.Services.AddTransient<IPlaylistRepos, PlaylistRepos>();

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

app.MapControllers();

app.Run();
