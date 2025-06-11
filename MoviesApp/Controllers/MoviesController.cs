using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.ViewModels;
using Movies.Models;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Controllers
{
    public class MoviesController : Controller
    {
        //Using WebAPIExecutor ===========================================
        //private readonly IWebApiExecutor _webApiExecutor;

        // Using Services ===========================================
        private readonly IMovieService _movieService;
        private readonly IPlaylistService _playlistService;
        private readonly IPlaylistMovieService _playlistMovieService;

        // Using Repositories ===========================================
        //private readonly IMovieRepos _movieRepos;
        //private readonly IPlaylistRepos _playlistRepos;
        //private readonly IPlaylistMovieRepos _playlistMovieRepos;
        private readonly IPhotoService _photoService; //Cloudnary 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoviesController(
             IMovieService movieService
            , IPlaylistService playlistService
            , IPlaylistMovieService playlistMovieService
            
            //, IWebApiExecutor webApiExecutor
            
            , IPhotoService photoService
            , IHttpContextAccessor httpContextAccessor
            )
        {
            _movieService = movieService;
            _playlistService = playlistService;
            _playlistMovieService = playlistMovieService;
            
        //    _webApiExecutor = webApiExecutor;

            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string search)
        {
            ViewBag.Message = "";
            IEnumerable<MovieDto> movieList;

            if (!string.IsNullOrEmpty(search))
            {
                //movieList = await _movieRepos.GetByName(search).ConfigureAwait(false);
                movieList = await _movieService.GetByName(search);

                if (movieList.Count() == 0)
                {
                    //movieList = await _movieRepos.GetAll();
                    movieList = await _movieService.GetAll();
                    //movieList = await _webApiExecutor.InvokeGet<List<MovieDto>>("Movies");

                    ViewBag.Message = "There are no movies with that Name!";
                }
            }
            else
            {
                //movieList = await _movieRepos.GetAll();
                movieList = await _movieService.GetAll();
                //movieList = await _webApiExecutor.InvokeGet<List<MovieDto>>("Movies/GetAll");
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
                var playlists = await _playlistService.GetAllByUser(user).ConfigureAwait(false);

                if (playlists.Any())
                {
                    ViewData["playlistName"] = new SelectList(
                    await _playlistService.GetAllByUser(user).ConfigureAwait(false), "Id", "Name");
                }
                else
                {
                    ViewBag.playlistName = null;
                    ViewBag.Message = "You have no Playlists. Please create one.";
                }
            }

            var movie = await _movieService.GetByIdNoTracking(id);
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
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("", "Failed to Add Movie");
            //    return View("Details", movieVM);
            //}

            //var movie = await _movieService.GetById(movieVM.Id);
            //if (movie == null)
            //{
            //    ModelState.AddModelError("", "Movie not found");
            //    return View("Details", movieVM);
            //}

            var newMovie = new PlaylistMovieDto
            {
                MovieId = movieVM.Id,
                PlaylistId = movieVM.PlaylistId
            };

            //var movieExist = _playlistMovieRepos.MovieInPlaylist(newMovie);
            
            var movieExist = await _playlistMovieService.MovieInPlaylist(movieVM.PlaylistId, movieVM.Id);
            if ( movieExist )
            {
                TempData["error"] = "Movie is already in Playlist!";
            }
            else
            {
                //_playlistMovieRepos.Add(newMovie);
                await _playlistMovieService.Add(newMovie);
                TempData["success"] = "Movie added to Playlist";
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
                var movie = new MovieDto
                {
                    Title = movieVM.Title,
                    Description = movieVM.Description,
                    Genre = movieVM.Genre,
                    Age = movieVM.Age,
                    PictUrl = result.Url.ToString(),
                    Active = true
                };

                await _movieService.Add(movie);
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
        public async Task<IActionResult> Edit(int id)
        {
            MovieDto movie = await _movieService.GetById(id);

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

            var movie = await _movieService.GetByIdNoTracking(id);

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

            await _movieService.Update(id, editMovie);
            TempData["success"] = "Movie details updated";

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

            var movie = await _movieService.GetById(id);
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
            var movie = await _movieService.GetById(id);
            if (movie != null)
            {
                //_movieRepos.Delete(movie);
                await _movieService.Delete(id);
                TempData["success"] = "Movie deleted";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}