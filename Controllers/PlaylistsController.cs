using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;
using MoviesApp.ViewModels;
using NuGet.ContentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;

namespace MoviesApp.Controllers
{
    [Authorize]
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistRepos _playlistRepos;
        private readonly IMovieRepos _movieRepos;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlaylistsController(IPlaylistRepos playlistRepos, IMovieRepos movieRepos, IHttpContextAccessor httpContextAccessor)
        {
            _playlistRepos = playlistRepos;
            _movieRepos = movieRepos;
            _httpContextAccessor = httpContextAccessor;
        }


        // GET: Playlists
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
                if( list.Count() == 0)
                {
                    ViewBag.Message = "There are no Playlists in your account.";
                }
            }
            return View(list);
        }

        // GET: Playlists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Playlist playlist = await _playlistRepos.GetByIdAsync(id);
            PlaylistMoviesVM newVm = new PlaylistMoviesVM()
            {
                Playlist = playlist,
                PlaylistId = playlist.Id,
                AppUser = playlist.AppUser,
                AppUserId = playlist.AppUserId,
                MoviesList = playlist.MovieList
            };
            
            return View(newVm);
        }

        //public IActionResult AddMovie(Playlist playlist)
        //{
            //if (ModelState.IsValid)
            //{
            //    Task<List<Movie>> movies = _movieRepos.GetAll();
            //    var newPlaylist = playlist;

            //    for( int i = 0; i++; i < movies.)
            //    {
            //        playlist.MoviesList.Add(item[i]);
            //    }
                
                //playlist.MoviesList.Add(movie);
                //_playlistRepos.Update(playlist);
          //  }
          //  return View();
        //}

        // GET: Playlists/Create
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

        // GET: Playlists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _playlistRepos.GetByIdAsync(id);
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

        // GET: Playlists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _playlistRepos.GetByIdAsync(id);
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
            var playlist = await _playlistRepos.GetByIdAsync(id);
            if (playlist != null)
            {
                _playlistRepos.Delete(playlist);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistExists(int id)
        {
            return _playlistRepos.PlaylistExists(id);
        }
    }
}
