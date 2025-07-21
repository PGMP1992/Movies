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

    public class UsersController(IUserRepos _repos) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repos.GetAll();

            if (users == null || !users.Any())
            {
                return NotFound(new ErrorResponse());
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ErrorResponse());
            }

            var user = await _repos.GetById(id);
            if (user == null)
            {
                return NotFound(new ErrorResponse());
            }

            return Ok(user.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdNoTracking(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ErrorResponse());
            }

            var user = await _repos.GetByIdNoTracking(id);
            if (user == null)
            {
                return NotFound(new ErrorResponse());
            }

            return Ok(user.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllPlaylists(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ErrorResponse());
            }

            var playlists = await _repos.GetAllPlaylists(id);
            if (playlists == null)
            {
                return NotFound(new ErrorResponse());
            }
            return Ok(playlists);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, AppUser user)
        {
            if (id != user.Id || user is null)
            {
                return BadRequest(new ErrorResponse());
            }

            var dbUser = await _repos.GetByIdNoTracking(id);
            if (dbUser == null)
            {
                return NotFound(new ErrorResponse());
            }

            _repos.Update(user);
            return Ok(user.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ErrorResponse());
            }
            var user = await _repos.GetById(id);
            if (user is null)
            {
                return NotFound(new ErrorResponse());
            }
            return Ok(_repos.Delete(user));
        }
    }
}
