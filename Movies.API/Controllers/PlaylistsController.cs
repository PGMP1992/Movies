using Microsoft.AspNetCore.Mvc;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace playlists.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PlaylistsController(IPlaylistRepos _repos) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var playlists = await _repos.GetAll();
            if (playlists == null || !playlists.Any())
            {
                return NotFound("No playlists available");
            }
            return Ok(playlists);
        }

        [HttpGet("{user}")]
        public async Task<IActionResult> GetAllByUser(string user)
        {
            var playlists = await _repos.GetAllByUser(user);
            if (playlists == null || !playlists.Any())
            {
                return NotFound("No playlists available");
            }
            return Ok(playlists);
        }

        // GET: playlistsController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var playlist = await _repos.GetById(id);
            if (playlist == null)
            {
                return NotFound("playlist doesn't exist!");
            }
            return Ok(playlist.ToDto());
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
        public async Task<IActionResult> Post(Playlist playlist)
        {
            var newplaylist = _repos.Add(playlist);
            if (newplaylist == false)
            {
                return BadRequest("Could not create playlist");
            }
            return CreatedAtAction(nameof(GetById), new { id = playlist.Id }, playlist);
        }

        // PUT: playlistsController/Edit/5
        [HttpPut]
        public async Task<IActionResult> Put(Playlist playlist)
        {
            if (playlist is null)
            {
                return NotFound("Playlist doesn't exist!");
            }

            var existingplaylist = await _repos.GetByIdNoTracking(playlist.Id);
            if (existingplaylist is null)
            {
                return NotFound("Playlist doesn't exist!");
            }

            _repos.Update(playlist);
            return Ok(playlist.ToDto());
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
