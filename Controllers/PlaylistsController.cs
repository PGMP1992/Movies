using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.DataSource.Repos.Interfaces;
using Movies.Models;
using Movies.Models.ViewModels;
using MoviesApp.Data;
using MoviesApp.DTOs;
using MoviesApp.Services;

namespace MoviesApp.Controllers
{
    [Authorize]
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistRepos _playlistRepos;
        private readonly IMovieRepos _movieRepos;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMovieService _movieService;
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IPlaylistRepos playlistRepos
            , IMovieRepos movieRepos
            , IHttpContextAccessor httpContextAccessor
            , IMovieService movieService
            , IPlaylistService playlistService
        )
        {
            _playlistRepos = playlistRepos;
            //_movieRepos = movieRepos;
            _httpContextAccessor = httpContextAccessor;

            _movieService = movieService;
            _playlistService = playlistService;
        }

        // GET: Playlists ---------------------------------------------------
        public async Task<IActionResult> Index()
        {
            IEnumerable<PlaylistDto> list;

            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
            {
                //IEnumerable<Playlist> playlists = await _playlistRepos.GetAll();
                IEnumerable<PlaylistDto> playlists = await _playlistService.GetAll();
                list = playlists;
                ViewBag.Message = "";
            }
            else
            {
                var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                //IEnumerable<Playlist> playlists = await _playlistRepos.GetAllByUser(curUserId);
                IEnumerable<PlaylistDto> playlists = await _playlistService.GetAllByUser(curUserId);
                list = playlists;
                
                if (list.Count() == 0)
                {
                    ViewBag.Message = "There are no Playlists in your account.";
                }
            }
            return View(list);
        }

        // GET: Playlists/Details/5 -------------------------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _playlistRepos.GetByIdNoTracking(id);

            PlaylistMoviesVM newVm = new PlaylistMoviesVM()
            {
                Playlist = playlist,
                PlaylistId = playlist.Id,
                AppUser = playlist.AppUser,
                AppUserId = playlist.AppUserId,
                Movies = playlist.Movies
            };

            return View(newVm);
        }

        public async Task<IActionResult> RemoveMovie(int playlistId, int movieId)
        {
            var movie = await _movieRepos.GetById(movieId);
            //var movie = await _movieService.Get(movieId);
            var playlist = await _playlistRepos.GetById(playlistId);

            if (playlist != null)
            {
                playlist.Movies.Remove(movie);
                _playlistRepos.Update(playlist);
                TempData["success"] = "Movie removed from Playlist";
            }

            PlaylistMoviesVM newVm = new PlaylistMoviesVM()
            {
                Playlist = playlist,
                PlaylistId = playlist.Id,
                AppUser = playlist.AppUser,
                AppUserId = playlist.AppUserId,
                Movies = playlist.Movies
            };

            return View("Details", newVm);
        }

        // GET: Playlists/Create -------------------------------------------
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var playlist = new PlaylistDto { 
                AppUserId = curUserId 
            };

            return View(playlist);
        }

        // POST: Playlists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                playlist.AppUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                _playlistRepos.Add(playlist);
                TempData["success"] = "Playlist created";
                return RedirectToAction(nameof(Index));
            }
            return View(playlist);
        }

        // GET: Playlists/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _playlistRepos.GetByIdNoTracking(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        // POST: Playlists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Playlist playlist)
        {
            if (id != playlist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    playlist.AppUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                    _playlistRepos.Update(playlist);
                    TempData["success"] = "Playlist updated";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistExists(playlist.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(playlist);
        }

        // GET: Playlists/Delete/5 ------------------------------------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _playlistRepos.GetByIdNoTracking(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        // POST: Playlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playlist = await _playlistRepos.GetByIdNoTracking(id);
            if (playlist != null)
            {
                _playlistRepos.Delete(playlist);
                TempData["success"] = "Playlist deleted";
            }

            return RedirectToAction(nameof(Index));
        }

        // -------------------------------------------------------
        private bool PlaylistExists(int id)
        {
            return _playlistRepos.PlaylistExists(id);
        }
    }
}
