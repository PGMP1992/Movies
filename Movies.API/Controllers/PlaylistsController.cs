using Microsoft.AspNetCore.Mvc;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace playlists.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PlaylistsController(IPlaylistRepos _playlistRepos) : ControllerBase
    {
        //private readonly IPlaylistRepos _playlistRepos = playlistRepos;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var playlists = await _playlistRepos.GetAll();
            if (playlists == null || !playlists.Any())
            {
                return NotFound("No playlists available");
            }
            return Ok(playlists);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByUser(string user)
        {
            var playlists = await _playlistRepos.GetAllByUser(user);
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
            var playlist = await _playlistRepos.GetById(id);
            if (playlist == null)
            {
                return NotFound("playlist doesn't exist!");
            }
            return Ok(playlist.ToPlaylistDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdNoTracking(int id)
        {
            var playlist = await _playlistRepos.GetByIdNoTracking(id);
            if (playlist == null)
            {
                return NotFound("playlist doesn't exist!");
            }
            return Ok(playlist.ToPlaylistDto());
        }

        // POST: playlistsController/Create
        [HttpPost]
        public async Task<IActionResult> Post(Playlist playlist)
        {
            var newplaylist = _playlistRepos.Add(playlist);
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

            var existingplaylist = await _playlistRepos.GetByIdNoTracking(playlist.Id);
            if (existingplaylist is null)
            {
                return NotFound("Playlist doesn't exist!");
            }

            _playlistRepos.Update(playlist);
            return Ok(playlist);
        }

        // DELETE: playlistsController/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var playlist = await _playlistRepos.GetById(id);
            if (playlist is null)
            {
                return NotFound("playlist doesn't exist!");
            }
            var deleted = _playlistRepos.Delete(playlist);
            return Ok(deleted);
        }
    }
}
