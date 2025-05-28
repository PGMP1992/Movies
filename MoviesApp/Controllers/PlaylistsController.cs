using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.DataAccess.Data;
using Movies.DataAccess.ViewModels;
using Movies.Models;
using MoviesApp.Services;

namespace MoviesApp.Controllers
{
    [Authorize]
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly IMovieService _movieService;
        private readonly IPlaylistMovieService _playlistMovieService;

        //private readonly IPlaylistRepos _playlistRepos;
        //private readonly IMovieRepos _movieRepos;
        //private readonly IPlaylistMovieRepos _playlistMovieRepos;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlaylistsController(
            IPlaylistService playlistService
            , IMovieService movieService
            , IPlaylistMovieService playlistMovieService

            //,IPlaylistRepos playlistRepos
            //, IMovieRepos movieRepos
            //, IPlaylistMovieRepos playlistMovieRepos
            , IHttpContextAccessor httpContextAccessor
        )
        {
            _playlistService = playlistService;
            _movieService = movieService;
            _playlistMovieService = playlistMovieService;

            //_playlistRepos = playlistRepos;
            //_movieRepos = movieRepos;
            //_playlistMovieRepos = playlistMovieRepos;
            _httpContextAccessor = httpContextAccessor;
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
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PlaylistMoviesVM newVm = new PlaylistMoviesVM()
            {
                Playlist = await _playlistService.GetById(id),
                //Playlists = await _playlistMovieRepos.GetByPlaylist(id)
                Playlists = await _playlistMovieService.GetByPlaylist(id),
            };

            return View(newVm);
        }

        public async Task<IActionResult> RemoveMovie(int playlistId, int movieId)
        {
            //var obj = await _playlistMovieRepos.GetByContent(playlistId, movieId);
            var obj = await _playlistMovieService.GetByContent(playlistId, movieId);

            if (obj != null)
            {
                //_playlistMovieRepos.Delete(obj);
                await _playlistMovieService.Delete(obj.Id);
                TempData["success"] = "Movie deleted from Playlist";
            }

            PlaylistMoviesVM newVm = new PlaylistMoviesVM()
            {
                Playlist = await _playlistService.GetById(playlistId),
                //Playlists = await _playlistMovieRepos.GetByPlaylist(playlistId)
                Playlists = await _playlistMovieService.GetByPlaylist(playlistId)
            };

            return View("Details", newVm);
        }

        // GET: Playlists/Create -------------------------------------------
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var playlist = new PlaylistDto { AppUserId = curUserId };

            return View(playlist);
        }

        // POST: Playlists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AppUserId")] PlaylistDto playlist)
        {
            if (ModelState.IsValid)
            {
                //playlist.AppUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                //_playlistRepos.Add(playlist);
                await _playlistService.Add(playlist);
                TempData["success"] = "Playlist created";
                return RedirectToAction(nameof(Index));
            }
            return View(playlist);
        }

        // GET: Playlists/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var playlist = await _playlistRepos.GetByIdNoTracking(id);
            var playlist = await _playlistService.GetByIdNoTracking(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        // POST: Playlists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name, AppUserId")] PlaylistDto playlist)
        {
            if (id != playlist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //playlist.AppUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                //_playlistRepos.Update(playlist);
                await _playlistService.Update(playlist);
                TempData["success"] = "Playlist updated";
                return RedirectToAction(nameof(Index));
            }
            return View(playlist);
        }

        // GET: Playlists/Delete/5 ------------------------------------------------
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var playlist = await _playlistRepos.GetByIdNoTracking(id);
            var playlist = await _playlistService.GetByIdNoTracking(id);
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
            //var playlist = await _playlistRepos.GetByIdNoTracking(id);
            var playlist = await _playlistService.GetByIdNoTracking(id);
            if (playlist != null)
            {
                //_playlistRepos.Delete(playlist);
                await _playlistService.Delete(playlist.Id);
                TempData["success"] = "Playlist deleted";
            }

            return RedirectToAction(nameof(Index));
        }

        // -------------------------------------------------------
        //private bool PlaylistExists(int id)
        //{
        //    return _playlistRepos.PlaylistExists(id);
        //    //return _playlistService.Exists(id);
        //}
    }
}
