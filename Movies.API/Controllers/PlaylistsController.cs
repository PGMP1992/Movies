using Microsoft.AspNetCore.Mvc;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Models;
using Movies.Models;

namespace Movies.API.Controllers
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
                //return NotFound("No playlists available");
                return NotFound(new ErrorModel()
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
            var playlists = await _repos.GetAllByUser(user);
            if (playlists == null || !playlists.Any())
            {
                //return NotFound("No playlists available");
                return NotFound(new ErrorModel()
                {
                    ErrorMessage = "No Playlists available",
                    StatusCode = StatusCodes.Status404NotFound
                });
                
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
                //return NotFound("playlist doesn't exist!");
                return NotFound(new ErrorModel()
                {
                    ErrorMessage = "No Playlists available",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(playlist.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdNoTracking(int id)
        {
            var playlist = await _repos.GetByIdNoTracking(id);
            if (playlist == null)
            {
                //return NotFound("playlist doesn't exist!");
                return NotFound(new ErrorModel()
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
            var newplaylist = _repos.Add(playlist);
            if (newplaylist == false)
            {
                //return BadRequest("Could not create playlist");
                return BadRequest(new ErrorModel()
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
            if( id != playlist.Id)
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid playlist",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            if (playlist is null)
            {
                //return NotFound("Playlist doesn't exist!");
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid playlist",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var dbPlaylist = await _repos.GetByIdNoTracking(playlist.Id);
            if (dbPlaylist is null)
            {
                //return NotFound("Playlist doesn't exist!");
                return NotFound(new ErrorModel()
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
            var playlist = await _repos.GetById(id);
            if (playlist is null)
            {
                //return NotFound("playlist doesn't exist!");
                return NotFound(new ErrorModel()
                {
                    ErrorMessage = "Playlist doesn't exist!",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            //var deleted = _repos.Delete(playlist);
            return Ok( _repos.Delete(playlist));
        }
    }
}
