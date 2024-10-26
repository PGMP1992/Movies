using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Models.ViewModels;
using MoviesApp.Repos.Interfaces;

namespace MoviesApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepos _movieRepos;
        private readonly IPlaylistRepos _playlistRepos;
        private readonly IPlaylistMovieRepos _playlistMovieRepos;
        private readonly IPhotoService _photoService; //Cloudnary 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoviesController(
            IMovieRepos movieRepos,
            IPlaylistRepos playlistRepos,
            IPlaylistMovieRepos playlistMovieRepos,
            IPhotoService photoService, 
            IHttpContextAccessor httpContextAccessor)
        {
            _movieRepos = movieRepos;
            _playlistRepos = playlistRepos;
            _playlistMovieRepos = playlistMovieRepos;
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

        // Added - Add Movie to Playlist 
        // GET: Movies/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _httpContextAccessor.HttpContext.User.GetUserId();
            var movie = await _movieRepos.GetByIdNoTracking(id);
            var movies = await _playlistMovieRepos.GetAll();
            
            if (movie == null)
            {
                return NotFound();
            }

            PlaylistMovie newPM = new PlaylistMovie()
            {
                MovieId = movie.Id,
            };

            ViewBag.Message = "";
            //ViewData["playlistName"] = new SelectList(await _playlistRepos.GetAllByUserName(user)
            //        .ConfigureAwait(false), "Id", "Name");

            return View(newPM);
        }

        [Authorize]
        // POST: Movies/AddMovie/5 
        [HttpPost]
        public async Task<IActionResult> Details(PlaylistMovie pm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Add Movie");
                return View("Details", pm);
            }

            var newMovie = await _movieRepos.GetByIdNoTracking(pm.MovieId);
            var playlist = await _playlistRepos.GetById(pm.PlaylistId);

            // Check if Movie in Playlist.MovieList
            //if ( _playlistMovieRepos.FirstOrDefault(pm.PlaylistId) != null)
            //{
            //    ViewBag.Message = "Film is already in this Playlist!";
            //}
            //else
            //{

            _playlistRepos.Update(playlist);
            ViewBag.Message = "Movie Added to Playlist.";
            //} 
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
            var movie = await _movieRepos.GetById(id);

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
            }
            return RedirectToAction(nameof(Index));
        }
    }
}