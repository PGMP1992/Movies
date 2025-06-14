﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.ViewModels;
using Movies.Models;
using MoviesApp.Services;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Controllers
{
    public class MoviesController : ControllerBase
    {
        private readonly IWebApiExecutor _webApiExecutor;
        private readonly IPhotoService _photoService; //Cloudnary 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoviesController(
            IWebApiExecutor webApiExecutor
            ,IPhotoService photoService
            ,IHttpContextAccessor httpContextAccessor
            ) 
        {
            _webApiExecutor = webApiExecutor;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(string search)
        {
            ViewBag.Message = "";
            IEnumerable<MovieDto> movieList;

            if (!string.IsNullOrEmpty(search))
            {
                var result = await _webApiExecutor.InvokeGet<List<MovieDto>>($"Movies/GetByName/{search}");
                movieList = result ?? new List<MovieDto>(); 

                if (!movieList.Any())
                {
                    movieList = await _webApiExecutor.InvokeGet<List<MovieDto>>("Movies/GetAll") ?? new List<MovieDto>();
                    ViewBag.Message = "There are no movies with that Name!";
                }
            }
            else
            {
                movieList = await _webApiExecutor.InvokeGet<List<MovieDto>>("Movies/GetAll") ?? new List<MovieDto>();
            }

            return View("Index", movieList);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Message = "";

            if (User.Identity.IsAuthenticated)
            {
                var user = _httpContextAccessor.HttpContext.User.GetUserId();
                //var playlists = await _playlistService.GetAllByUser(user).ConfigureAwait(false);
                var playlists = await _webApiExecutor.InvokeGet<List<PlaylistDto>>($"Playlists/GetAllByUser/{user}");

                if (playlists.Any())
                {
                    ViewData["playlistName"] = new SelectList(
                        //await _playlistService.GetAllByUser(user).ConfigureAwait(false), "Id", "Name");
                        await _webApiExecutor.InvokeGet<List<PlaylistDto>>($"Playlists/GetAllByUser/{user}")
                            , "Id", "Name");
                }
                else
                {
                    ViewBag.playlistName = null;
                    ViewBag.Message = "You have no Playlists. Please create one.";
                }
            }

            //var movie = await _movieService.GetByIdNoTracking(id);
            var movie = await _webApiExecutor.InvokeGet<MovieDto>($"Movies/GetByIdNoTracking/{id}");
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
                PictUrl = movie.PictUrl,
                Active = movie.Active
            };

            return View(movieVM);
        }

        // POST: Movies/Details/5
        // Add Movie to Playlists 
        [HttpPost]
        public async Task<IActionResult> Details(AddMovieVM movieVM)
        {
            var newMovie = new PlaylistMovieDto
            {
                MovieId = movieVM.Id,
                PlaylistId = movieVM.PlaylistId
            };

            //var movieExist = await _playlistMovieService.MovieInPlaylist(movieVM.PlaylistId, movieVM.Id);
            var movieExist = await _webApiExecutor.InvokeGet<bool>($"PlaylistMovies/MovieInPlaylist/{movieVM.PlaylistId}/{movieVM.Id}");

            if (movieExist)
            {
                TempData["error"] = "Movie is already in Playlist!";
            }
            else
            {
                //_playlistMovieRepos.Add(newMovie);
                //await _playlistMovieService.Add(newMovie);
                try
                {
                    await _webApiExecutor.InvokePost("PlaylistMovies/Post", newMovie);
                    TempData["success"] = "Movie added to Playlist";
                }
                catch (WebApiException ex)
                {
                    HandleApiException(ex);
                    TempData["error"] = "Movie could not be added to Playlist!";
                }
            }
            return RedirectToAction(nameof(Details));
        }

        [Authorize]
        public IActionResult Create()
        {
            var movieVM = new CreateMovieVM();
            return View(movieVM);
        }

        // POST: Movies/Create
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieVM movieVM)
        {
            if (ModelState.IsValid)
            {
                if (movieVM.Image == null)
                {
                    ModelState.AddModelError("Image", "Please upload a photo");
                    return View(movieVM);
                }

                var result = await _photoService.AddPhotoAsync(movieVM.Image);
                var movie = new MovieDto
                {
                    Title = movieVM.Title,
                    Description = movieVM.Description,
                    Genre = movieVM.Genre,
                    Age = movieVM.Age,
                    PictUrl = result.Url.ToString(),
                    Active = true
                };

                try
                {
                    //await _movieService.Add(movie);
                    var response = await _webApiExecutor.InvokePost("Movies/Post", movie);
                    if (response != null)
                    {
                        TempData["success"] = "Movie created!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (WebApiException ex)
                {
                    HandleApiException(ex);
                    TempData["error"] = "Movie could not be created!";
                    return View(movieVM);
                }
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed. ");
            }
            return View(movieVM);
        }

        [Authorize]
        // GET: Movies/Edit/5 ------------------------------------------------------
        public async Task<IActionResult> Edit(int id)
        {
            //MovieDto movie = await _movieService.GetById(id);
            var movie = await _webApiExecutor.InvokeGet<MovieDto>($"Movies/GetById/{id}");

            if (movie == null)
            {
                return NotFound();
            }

            var movieVM = new EditMovieVM
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                Age = movie.Age,
                PictUrl = movie.PictUrl,
                Active = movie.Active
            };
            return View(movieVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditMovieVM movieVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit Movie");
                return View("Edit", movieVM);
            }

            //var movie = await _movieService.GetByIdNoTracking(id);
            var movie = await _webApiExecutor.InvokeGet<MovieDto>($"Movies/GetByIdNoTracking/{id}");

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

            var editMovie = new MovieDto
            {
                Id = id,
                Title = movieVM.Title,
                Description = movieVM.Description,
                Genre = movieVM.Genre,
                Age = movieVM.Age,
                PictUrl = movieVM.PictUrl,
                Active = movieVM.Active,
            };

            // Just change Active status if Admin user.
            if (!User.IsInRole("admin"))
            {
                editMovie.Active = true;
            }

            try
            {
                //await _movieService.Update(id, editMovie);
                await _webApiExecutor.InvokePut($"Movies/Put/{id}", editMovie);
                TempData["success"] = "Movie details updated";
            }
            catch (WebApiException ex)
            {
                HandleApiException(ex);
                TempData["error"] = "Movie could not be updated!";
                return View("Edit", movieVM);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        // GET: Movies/Delete/5 --------------------------------------------------------
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var movie = await _movieService.GetById(id);
            var movie = await _webApiExecutor.InvokeGet<MovieDto>($"Movies/GetByIdNoTracking/{id}");
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
            try
            {
                //var movie = await _movieService.GetById(id);
                var movie = await _webApiExecutor.InvokeGet<MovieDto>($"Movies/GetById/{id}");
                if (movie != null)
                {
                    var photoResult = await _photoService.DeletePhotoAsync(movie.PictUrl);
                    if (photoResult.Error != null)
                    {
                        ModelState.AddModelError("Image", "Failed to Delete image for Movie");
                        return View("Delete");
                    }

                    //_movieRepos.Delete(movie);
                    //await _movieService.Delete(id);
                    await _webApiExecutor.InvokeDelete($"Movies/Delete/{id}");
                    TempData["success"] = "Movie deleted";
                }
            }
            catch (WebApiException ex)
            {
                HandleApiException(ex);
                TempData["error"] = "Movie could not be deleted!";
                return View("Delete");
            }
            return RedirectToAction(nameof(Index));
        }


    }
}