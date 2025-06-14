﻿using Microsoft.AspNetCore.Mvc;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Models;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByPlaylist(int id)
        {
            var list = await _repos.GetByPlaylist(id);
            if (list == null || !list.Any())
            {
                return NotFound("No playlists found");
            }
            return Ok(list);
        }

        [HttpGet("{playlistId}/{movieId}")]
        public async Task<IActionResult> GetByContent(int playlistId, int movieId)
        {
            var obj = await _repos.GetByContent(playlistId, movieId);
            if (obj == null)
            {
                return NotFound("No Movie found in this Playlist");
            }
            
            return Ok(obj.ToDto());
        }

        [HttpGet("{playlistId}/{movieId}")]
        public async Task<IActionResult> MovieInPlaylist(int playlistId, int movieId)
        {
            var obj = _repos.MovieInPlaylist(playlistId, movieId);
            if (obj == false)
            {
                return NotFound("Movie is not in playlist.");
            }
            return Ok(obj);
        }

        // GET: playlistsController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var obj = await _repos.GetById(id);
            if (obj == null)
            {
                return NotFound("playlist doesn't exist!");
            }
            
            return Ok(obj.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdNoTracking(int id)
        {
            var obj = await _repos.GetByIdNoTracking(id);
            if (obj == null)
            {
                return NotFound("playlist doesn't exist!");
            }
            
            return Ok(obj.ToDto());
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

        //[HttpGet("{obj}")]
        //public async Task<IActionResult> MovieInPlaylist(PlaylistMovie obj)
        //{
        //    var playlist = _repos.MovieInPlaylist(obj);
        //    if (playlist is false)
        //    {
        //        return NotFound("Movie not in Playlist!");
        //    }
            
        //    return Ok(obj);
        //}
    }
}
