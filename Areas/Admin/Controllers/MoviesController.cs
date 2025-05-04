using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movies.DataSource.Repos.Interfaces;
using Movies.Models;
using Movies.Models.ViewModels;
using Movies.Utility;
using MoviesApp.Data;


namespace MoviesApp.Areas.Admin.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepos _movieRepos;
        private readonly IPlaylistRepos _playlistRepos;
        private readonly IPhotoService _photoService; //Cloudnary 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoviesController(IMovieRepos movieRepos
            , IPlaylistRepos playlistRepos
            , IPhotoService photoService
            , IHttpContextAccessor httpContextAccessor
            )
        {
            _movieRepos = movieRepos;
            _playlistRepos = playlistRepos;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string search)
        {
            var movies = await _movieRepos.GetAll(true); // Get only active movies

            if (User.IsInRole("admin"))
            {
                movies = await _movieRepos.GetAll(false); // Get all movies 
            }

            ViewBag.Message = "";

            if (!string.IsNullOrEmpty(search))
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

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Message = "";

            if (User.Identity.IsAuthenticated)
            {
                var user = _httpContextAccessor.HttpContext.User.GetUserId();
                var playlists = await _playlistRepos.GetAllByUser(user).ConfigureAwait(false);

                if (playlists.Count > 0)
                {
                    ViewData["playlistName"] = new SelectList(
                        await _playlistRepos.GetAllByUser(user)
                            .ConfigureAwait(false), "Id", "Name");
                }
                else
                {
                    ViewBag.playlistName = null;
                    ViewBag.Message = "You have no Playlists. Please create one.";
                }
            }

            var movie = await _movieRepos.GetByIdNoTracking(id);

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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Add Movie");
                return View("Details", movieVM);
            }

            var movie = await _movieRepos.GetById(movieVM.Id);
            if (movie == null)
            {
                ModelState.AddModelError("", "Movie not found");
                return View("Details", movieVM);
            }

            // Has to use GetByIdNoTracking
            var playlist = await _playlistRepos.GetById(movieVM.PlaylistId);

            if (playlist != null) //Check if No Playlists
            {
                if (!playlist.Movies.Contains(movie))
                {
                    playlist.Movies.Add(movie);
                    _playlistRepos.Update(playlist);
                    TempData["success"] = "Movie added to Playlist";
                }
                else
                {
                    TempData["error"] = "Movie is already in Playlist";
                }
            }
            return RedirectToAction(nameof(Details));
        }

        [Authorize]
        // GET: Movies/Create ------------------------------------------------------
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
                var movie = new Movie
                {
                    Title = movieVM.Title,
                    Description = movieVM.Description,
                    Genre = movieVM.Genre,
                    Age = movieVM.Age,
                    PictUrl = result.Url.ToString(),
                    Active = true
                };

                _movieRepos.Add(movie);
                TempData["success"] = "Movie created";

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
            var movie = await _movieRepos.GetById(id);

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
        // POST: Movies/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditMovieVM movieVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit Movie");
                return View("Edit", movieVM);
            }

            var movie = await _movieRepos.GetByIdNoTracking(id);

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
                PictUrl = movieVM.PictUrl,
                Active = movieVM.Active,
            };

            // Just change Active status if Admin user.
            if (!User.IsInRole("admin"))
            {
                editMovie.Active = true;
            }

            _movieRepos.Update(editMovie);
            TempData["success"] = "Movie details updated";

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

            var movie = await _movieRepos.GetById(id);
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
            var movie = await _movieRepos.GetById(id);
            if (movie != null)
            {
                _movieRepos.Delete(movie);
                TempData["success"] = "Movie deleted";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}