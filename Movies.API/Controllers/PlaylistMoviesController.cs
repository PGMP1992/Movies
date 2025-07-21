using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Filters;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Models;

namespace Movies.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [JwtTokenAuth] // Custom filter for JWT authentication

    public class PlaylistMoviesController(IPlaylistMovieRepos _repos) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByPlaylist(int id)
        {
            return Ok(await _repos.GetByPlaylist(id));
        }

        [HttpGet("{playlistId}/{movieId}")]
        public async Task<IActionResult> GetByContent(int playlistId, int movieId)
        {
            var obj = await _repos.GetByContent(playlistId, movieId);
            if (obj == null)
            {
                return NotFound(new ErrorResponse());
            }
            return Ok(obj.ToDto());
        }

        [HttpGet("{playlistId}/{movieId}")]
        public bool MovieInPlaylist(int playlistId, int movieId)
        {
            return _repos.MovieInPlaylist(playlistId, movieId);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var obj = await _repos.GetById(id);
            if (obj == null)
            {
                return NotFound(new ErrorResponse());
            }
            return Ok(obj.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Post(PlaylistMovie obj)
        {
            var newplaylist = _repos.Add(obj);
            if (newplaylist == false)
            {
                return BadRequest(new ErrorResponse());
            }
            return CreatedAtAction(nameof(GetById), new { id = obj.Id }, obj);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var playlist = await _repos.GetById(id);
            if (playlist is null)
            {
                return NotFound(new ErrorResponse());
            }
            var deleted = _repos.Delete(playlist);
            return Ok(deleted);
        }
    }
}
