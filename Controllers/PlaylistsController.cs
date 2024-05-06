using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Interfaces;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistRepos _playlistRepos;
        private readonly IMovieRepos _movieRepos;

        public PlaylistsController(IPlaylistRepos playlistRepos, IMovieRepos movieRepos)
        {
            _playlistRepos = playlistRepos;
            _movieRepos = movieRepos;
        }

        // GET: Playlists
        public async Task<IActionResult> Index()
        {
            IEnumerable<Playlist> playlists = await _playlistRepos.GetAll();
            return View(playlists);
        }

        // GET: Playlists/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            /* Using PlaylistMoviesVM ------------------------- 
            PlaylistMoviesVM playlistMoviesVM = new PlaylistMoviesVM
            {
               Playlist = await _playlistRepos.GetByIdAsync(id),
               Movies = await _movieRepos.GetAll()
            };
            */

            var playlist = await _playlistRepos.GetByIdAsync(id);

            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        // GET: Playlists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Playlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                //playlist.UserName = AppUser.
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        
        //public void AddMovie(int id, Movie movie)
        //{
        //    var playlist = _playlistRepos.GetPlaylistById(id);
        //    _playlistRepos.AddMovieToPlaylist(playlist, movie);
        //    _playlistRepos.Update(playlist);
        //}
    }
}
