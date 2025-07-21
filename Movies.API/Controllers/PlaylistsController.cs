using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Filters;
using Movies.API.Filters.Playlist;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Models;

namespace Movies.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [JwtTokenAuth]// Custom filter for JWT authentication

    public class PlaylistsController(IPlaylistRepos _repos) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var playlists = await _repos.GetAll();
            if (playlists == null || !playlists.Any())
            {
                return NotFound(new ErrorResponse());
            }
            return Ok(playlists);
        }

        [HttpGet("{user}")]
        public async Task<IActionResult> GetAllByUser(string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                return BadRequest(new ErrorResponse());
            }

            var playlists = await _repos.GetAllByUser(user);
            if (playlists == null || !playlists.Any())
            {
                return NotFound(new ErrorResponse());
            }
            return Ok(playlists);
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Play_ValidateIdFilterAttribute))]
        public async Task<IActionResult> GetById(int? id)
        {
            return Ok(HttpContext.Items["playlist"]);
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Play_ValidateIdFilterAttribute))]
        public async Task<IActionResult> GetByIdNoTracking(int? id)
        {
            return Ok(HttpContext.Items["playlist"]);
        }

        // POST: playlistsController/Create
        [HttpPost]
        [TypeFilter(typeof(Play_ValidateCreateFilterAttribute))]
        public async Task<IActionResult> Post(Playlist playlist)
        {
            _repos.Add(playlist);
            return CreatedAtAction(nameof(GetById), new { id = playlist.Id }, playlist);
        }

        // PUT: playlistsController/Edit/5
        [HttpPut("{id}")]
        //[TypeFilter(typeof(Play_ValidateIdFilterAttribute))]
        [TypeFilter(typeof(Play_ValidateUpdateFilterAttribute))]
        public async Task<IActionResult> Put(int id, Playlist playlist)
        {
            var dbPlay = await _repos.GetByIdNoTracking(playlist.Id);
            if (dbPlay is null)
            {
                return NotFound(new ErrorResponse());
            }
            _repos.Update(playlist);
            return NoContent();
        }

        // DELETE: playlistsController/Delete/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(Play_ValidateIdFilterAttribute))]
        public async Task<IActionResult> Delete(int id)
        {
            var playDelete = HttpContext.Items["playlist"] as Playlist;
            _repos.Delete(playDelete);
            return Ok(playDelete);
        }
    }
}
