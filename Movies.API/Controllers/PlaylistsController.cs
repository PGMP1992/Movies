using Microsoft.AspNetCore.Mvc;
using Movies.API.Filters;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Models;
using Movies.Models;

namespace Movies.API.Controllers
{
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
                return NotFound(new ErrorModelDto()
                {
                    ErrorMessage = "No Playlists available",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(playlists);
        }

        [HttpGet("{user}")]
        public async Task<IActionResult> GetAllByUser(string user)
        {
            if(string.IsNullOrEmpty(user))
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "User name cannot be null or empty",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var playlists = await _repos.GetAllByUser(user);
            if (playlists == null || !playlists.Any())
            {
                return NotFound(new ErrorModelDto()
                {
                    ErrorMessage = "No Playlists available",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(playlists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "Invalid playlist ID",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var playlist = await _repos.GetById(id);
            if (playlist == null)
            {
                return NotFound(new ErrorModelDto()
                {
                    ErrorMessage = "No Playlists available",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(playlist.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdNoTracking(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "Invalid playlist ID",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var playlist = await _repos.GetByIdNoTracking(id);
            if (playlist == null)
            {
                return NotFound(new ErrorModelDto()
                {
                    ErrorMessage = "No Playlists found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(playlist.ToDto());
        }

        // POST: playlistsController/Create
        [HttpPost]
        public async Task<IActionResult> Post(Playlist playlist)
        {
            if(playlist is null)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "Invalid playlist",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var newplaylist = _repos.Add(playlist);
            if (newplaylist == false)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "Could not create playlist",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return CreatedAtAction(nameof(GetById), new { id = playlist.Id }, playlist);
        }

        // PUT: playlistsController/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Playlist playlist)
        {
            if( id != playlist.Id 
                || id <= 0
                || playlist == null)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "Invalid playlist",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var dbPlaylist = await _repos.GetByIdNoTracking(playlist.Id);
            if (dbPlaylist is null)
            {
                return NotFound(new ErrorModelDto()
                {
                    ErrorMessage = "Playlist doesn't exist!",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            _repos.Update(playlist);
             return Ok(playlist.ToDto());
        }

        // DELETE: playlistsController/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id == null || id <= 0)
            {
                return BadRequest(new ErrorModelDto()
                {
                    ErrorMessage = "Invalid playlist ID",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            var playlist = await _repos.GetById(id);
            if (playlist is null)
            {
                return NotFound(new ErrorModelDto()
                {
                    ErrorMessage = "Playlist doesn't exist!",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok( _repos.Delete(playlist));
        }
    }
}
