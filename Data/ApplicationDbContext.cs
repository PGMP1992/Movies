﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Models;

namespace MoviesApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistMovie> PlaylistMovies { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // // Added to enable Identity - Not sure if that is a bug 
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Playlist>()
        //        .HasMany(x => x.MovieList)
        //        .WithMany(y => y.PlaylistList);
        //        //.UsingEntity(j => j.ToTable("MoviePlaylist"));

        //    modelBuilder.Entity<Movie>()
        //        .HasMany(e => e.PlaylistList)
        //        .WithMany(e => e.MovieList);
        //}
    }
}
