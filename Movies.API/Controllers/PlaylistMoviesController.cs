using Microsoft.AspNetCore.Mvc;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace Movies.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PlaylistMoviesController(IPlaylistMovieRepos _repos) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repos.GetAll();
            if (list == null || !list.Any())
            {
                return NotFound("No playlists available");
            }
            return Ok(list);
        }

        [HttpGet("{user}")]
        public async Task<IActionResult> GetAllByUser(string user)
        {
            var list = await _repos.GetAllByUser(user);
            if (list == null || !list.Any())
            {
                return NotFound("No playlists available");
            }
            return Ok(list);
        }

        // GET: playlistsController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var list = await _repos.GetById(id);
            if (list == null)
            {
                return NotFound("playlist doesn't exist!");
            }
            return Ok(list.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdNoTracking(int id)
        {
            var playlist = await _repos.GetByIdNoTracking(id);
            if (playlist == null)
            {
                return NotFound("playlist doesn't exist!");
            }
            return Ok(playlist.ToDto());
        }

        // POST: playlistsController/Create
        [HttpPost]
        public async Task<IActionResult> Post(PlaylistMovie obj)
        {
            var newplaylist = _repos.Add(obj);
            if (newplaylist == false)
            {
                return BadRequest("Could not create playlist");
            }
            return CreatedAtAction(nameof(GetById), new { id = obj.Id }, obj);
        }

        // PUT: playlistsController/Edit/5
        [HttpPut]
        public async Task<IActionResult> Put(PlaylistMovie obj)
        {
            if (obj is null)
            {
                return NotFound("Playlist doesn't exist!");
            }

            var dbplaylist = await _repos.GetByIdNoTracking(obj.Id);
            if (dbplaylist is null)
            {
                return NotFound("Playlist doesn't exist!");
            }

            _repos.Update(obj);
            return Ok(obj.ToDto());
        }

        // DELETE: playlistsController/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var playlist = await _repos.GetById(id);
            if (playlist is null)
            {
                return NotFound("playlist doesn't exist!");
            }
            var deleted = _repos.Delete(playlist);
            return Ok(deleted);
        }
    }
}
