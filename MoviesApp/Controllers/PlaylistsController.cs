using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.DataAccess.Data;
using Movies.DataAccess.ViewModels;
using Movies.Models;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Controllers
{
    [Authorize]
    public class PlaylistsController : ControllerBase
    {
        private readonly IWebApiExecutor _webApiExecutor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlaylistsController(
            IWebApiExecutor webApiExecutor
            , IHttpContextAccessor httpContextAccessor
        )
        {
            _webApiExecutor = webApiExecutor;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Playlists ---------------------------------------------------
        public async Task<IActionResult> Index()
        {
            IEnumerable<PlaylistDto> list;

            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
            {
                //IEnumerable<PlaylistDto> playlists = await _playlistService.GetAll();
                IEnumerable<PlaylistDto> playlists = await _webApiExecutor.InvokeGet<List<PlaylistDto>>("Playlists/GetAll");
                list = playlists;
                ViewBag.Message = "";
            }
            else
            {
                var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
                //IEnumerable<PlaylistDto> playlists = await _playlistService.GetAllByUser(curUserId);
                IEnumerable<PlaylistDto> playlists = await _webApiExecutor.InvokeGet<List<PlaylistDto>>($"Playlists/GetAllByUser/{curUserId}");
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
                //Playlist = await _playlistService.GetById(id),
                //Playlists = await _playlistMovieService.GetByPlaylist(id),
                Playlist = await _webApiExecutor.InvokeGet<PlaylistDto>($"Playlists/GetByIdNoTracking/{id}"),
                Playlists = await _webApiExecutor.InvokeGet<List<PlaylistMovieDto>>($"PlaylistMovies/GetByPlaylist/{id}")
            };
            return View(newVm);
        }

        public async Task<IActionResult> RemoveMovie(int playlistId, int movieId)
        {
            //var obj = await _playlistMovieService.GetByContent(playlistId, movieId);
            var obj = await _webApiExecutor.InvokeGet<PlaylistMovieDto>($"PlaylistMovies/GetByContent/{playlistId}/{movieId}");

            if (obj != null)
            {
                //await _playlistMovieService.Delete(obj.Id);
                await _webApiExecutor.InvokeDelete($"PlaylistMovies/Delete/{obj.Id}");
                TempData["success"] = "Movie deleted from Playlist";
            }

            PlaylistMoviesVM newVm = new PlaylistMoviesVM()
            {
                Playlist = await _webApiExecutor.InvokeGet<PlaylistDto>($"Playlists/GetById/{playlistId}"),
                //Playlists = await _playlistMovieService.GetByPlaylist(playlistId)
                Playlists = await _webApiExecutor.InvokeGet<List<PlaylistMovieDto>>($"PlaylistMovies/GetByPlaylist/{playlistId}")
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
                //await _playlistService.Add(playlist);
                await _webApiExecutor.InvokePost("Playlists/Post", playlist);
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

            //var playlist = await _playlistService.GetByIdNoTracking(id);
            var playlist = await _webApiExecutor.InvokeGet<PlaylistDto>($"Playlists/GetByIdNoTracking/{id}");
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
                //await _playlistService.Update(id, playlist);
                await _webApiExecutor.InvokePut($"Playlists/Put/{id}", playlist);
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

            //var playlist = await _playlistService.GetByIdNoTracking(id);
            var playlist = await _webApiExecutor.InvokeGet<PlaylistDto>($"Playlists/GetByIdNoTracking/{id}");
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
            //var playlist = await _playlistService.GetByIdNoTracking(id);
            var playlist = await _webApiExecutor.InvokeGet<PlaylistDto>($"Playlists/GetByIdNoTracking/{id}");
            if (playlist != null)
            {
                //await _playlistService.Delete(playlist.Id);
                await _webApiExecutor.InvokeDelete($"Playlists/Delete/{playlist.Id}");
                TempData["success"] = "Playlist deleted";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
