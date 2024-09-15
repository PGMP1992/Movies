using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos;
using MoviesApp.Repos.Interfaces;

namespace MoviesApp.Controllers
{
    [Authorize]
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistRepos _playlistRepos;
        private readonly IMovieRepos _movieRepos;
        private readonly IPlaylistMovieRepos _playlistMoviesRepos;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static Playlist _playlist;

        public PlaylistsController(IPlaylistRepos playlistRepos
            ,IMovieRepos movieRepos
            ,IPlaylistMovieRepos playlistMoviesRepos
            ,IHttpContextAccessor httpContextAccessor)
        {
            _playlistRepos = playlistRepos;
            _movieRepos = movieRepos;
            _playlistMoviesRepos = playlistMoviesRepos;
            _httpContextAccessor = httpContextAccessor;
            //_playlist = new Playlist();
        }

        // GET: Playlists ---------------------------------------------------
        public async Task<IActionResult> Index()
        {
            IEnumerable<Playlist> list;

            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
            {
                IEnumerable<Playlist> playlists = await _playlistRepos.GetAll();
                list = playlists;
                ViewBag.Message = "";
            }
            else
            {
                var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                IEnumerable<Playlist> playlists = await _playlistRepos.GetAllByUserName(curUserId);
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

            //Playlist playlist = await _playlistRepos.GetByIdAsync(id);
            _playlist = await _playlistRepos.GetById(id);
            
            
            PlaylistMoviesVM newVm = new PlaylistMoviesVM()
            {
                Playlist = _playlist,
                PlaylistId = _playlist.Id,
                AppUser = _playlist.AppUser,
                AppUserId = _playlist.AppUserId,
                //MovieList = _playlistMoviesRepos.GetByIdAsync(id) 
            };

            return View(newVm);
        }

        // POST: Playlists/RemoveMovie/5
        //[HttpPost] -It doen't work. Returns Error 405 Not Found
        public IActionResult RemoveMovie(int id)
        {
            var playlist = _playlist;
            var movie = _movieRepos.GetById(id);
            //playlist.MovieList.Remove(movie);
            _playlistRepos.Update(playlist);
            return RedirectToAction(nameof(Details));
        }

        // GET: Playlists/Create -------------------------------------------
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            //var createPlaylistMoviesVM = new PlaylistMoviesVM { AppUserId = curUserId };
            var playlist = new Playlist { AppUserId = curUserId };

            //return View(createPlaylistMoviesVM);
            return View(playlist);
            //return View();
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

            var playlist = await _playlistRepos.GetById(id);
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

            var playlist = await _playlistRepos.GetById(id);
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
            var playlist = await _playlistRepos.GetById(id);
            if (playlist != null)
            {
                _playlistRepos.Delete(playlist);
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
