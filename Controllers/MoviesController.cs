﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesApp.Models;
using MoviesApp.ViewModels;
using MoviesApp.Repos;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using MoviesApp.Repos.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MoviesApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepos _movieRepos;
        private readonly IPlaylistRepos _playlistRepos;
        private readonly IPhotoService _photoService; //Cloudnary 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoviesController(IMovieRepos movieRepos, IPlaylistRepos playlistRepos,
                IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _movieRepos = movieRepos;
            _playlistRepos = playlistRepos;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string search)
        {
            var movies = await _movieRepos.GetAll();
            ViewBag.Message = "";

            if (!String.IsNullOrEmpty(search))
            {
                var movies1 = await _movieRepos.GetByName(search);
                if (movies1.Count > 0)
                {
                    return View(movies1);
                }
                ViewBag.Message = "There are not movies with that Name.";
            }
            return View(movies);
        }

        // -----------------------------Fix This here ---------------------
        // Add to Playlist 
        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var user = _httpContextAccessor.HttpContext.User.GetUserId();
            var movie = await _movieRepos.GetByIdAsync(id);
            
            if (movie == null)
            {
                return NotFound();
            }
            
            ViewData["playlistName"] = new SelectList(await _playlistRepos.GetAllByUserName(user)
                    .ConfigureAwait(false), "Id", "Name");
            return View(movie);
        }


        // GET: Movies/Create ------------------------------------------------------
        public IActionResult Create()
        {
            var movieVM = new CreateMovieVM();
            return View(movieVM);
        }

        // POST: Movies/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieVM movieVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(movieVM.Image);
                var movie = new Movie
                {
                    Title = movieVM.Title,
                    Description = movieVM.Description,
                    Genre = movieVM.Genre,
                    Age = movieVM.Age,
                    PictUrl = result.Url.ToString()
                };

                _movieRepos.Add(movie);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed. ");
            }
            return View(movieVM);
        }

        // GET: Movies/Edit/5 ------------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            var movie = await _movieRepos.GetByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }
            var movieVM = new EditMovieVM()
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                Age = movie.Age,
                PictUrl = movie.PictUrl
            };
            return View(movieVM);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditMovieVM movieVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit Movie");
                return View("Edit", movieVM);
            }
            
            var movie = await _movieRepos.GetByIdAsyncNoTracking(id);
            
            if (movieVM.Image != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(movieVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Failed to upload image");
                    return View("Edit", movieVM);
                }

                if (!string.IsNullOrEmpty(movie.PictUrl))
                {
                    _ = _photoService.DeletePhotoAsync(movie.PictUrl);
                }

                movie.PictUrl = photoResult.Url.ToString();
                movieVM.PictUrl = movie.PictUrl;
            }

            var editMovie = new Movie
            {
                Id = id,
                Title = movieVM.Title,
                Description = movieVM.Description,
                Genre = movieVM.Genre,
                Age = movieVM.Age,
                PictUrl = movieVM.PictUrl
            };

            _movieRepos.Update(editMovie);
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Delete/5 --------------------------------------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepos.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        
        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _movieRepos.GetByIdAsync(id);
            if (movie != null)
            {
                _movieRepos.Delete(movie);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Movies/AddToPlaylist ----------------------------------------------------------
        [HttpPost, ActionName("AddToPlaylist")]
        public async Task<IActionResult> AddToPlaylist(int id, int playlistId)
        {
            var user = _httpContextAccessor.HttpContext.User.GetUserId();
            var movie = await _movieRepos.GetByIdAsync(id);
            var playlist = await _playlistRepos.GetByIdAsync(playlistId);
            // Get in Selected List 
            if (playlist != null)
            {
                if ( ! playlist.MoviesList.Contains(movie))
                {
                    playlist.MoviesList.Add(movie);
                    _playlistRepos.Update(playlist);
                };
            }
            
            // Get playlistId and update on both Movies and Playlist tables
            return RedirectToAction(nameof(Details));
        }
    }
}
