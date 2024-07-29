﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos;
using MoviesApp.Repos.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using MoviesApp.Models.ViewModels;

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
            var movie = await _movieRepos.GetByIdAsyncNoTracking(id);
            if (movie == null)
            {
                return NotFound();
            }

            AddMovieVM movieVM = new AddMovieVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                Age = movie.Age,
                PictUrl = movie.PictUrl
                //PlaylistSelect = await _playlistRepos.GetAllByUserName(user)
                //    .Select(i => new SelectListItem {
                //   Text  = i.Name,
                //   Value = i.Id.ToString()  
                //})
            };
            
            ViewData["playlistName"] = new SelectList(await _playlistRepos.GetAllByUserName(user)
                    .ConfigureAwait(false), "Id", "Name");

            return View(movieVM);
        }

        //[Authorize]
        // POST: Movies/AddMovie/5
        [HttpPost]
        public async Task<IActionResult> Details(AddMovieVM movieVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Add Movie");
                return View("Details", movieVM);
            }
            
            var movie = await _movieRepos.GetByIdAsyncNoTracking(movieVM.Id);
            var playlist = await _playlistRepos.GetByIdAsync(movieVM.PlaylistId);

            playlist.MovieList.Add(movie);

            _playlistRepos.Update(playlist);
            _playlistRepos.Save();

            return RedirectToAction(nameof(Details));
        }

        [Authorize]
        // GET: Movies/Create ------------------------------------------------------
        public IActionResult Create()
        {
            var movieVM = new CreateMovieVM();
            return View(movieVM);
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _movieRepos.GetByIdAsync(id);
            if (movie != null)
            {
                _movieRepos.Delete(movie);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}