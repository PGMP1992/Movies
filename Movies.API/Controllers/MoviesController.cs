using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Filters;
using Movies.API.Filters.Movie;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Models;

namespace Movies.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [JwtTokenAuth] // Custom filter for JWT authentication

    public class MoviesController(IMovieRepos _repos) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repos.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActive()
        {
            return Ok(await _repos.GetAllActive());
        }

        [HttpGet("{search}")]
        //[TypeFilter(typeof(Movie_ValidateSearchFilterAttribute))]
        public async Task<IActionResult> GetByName(string search)
        {
            return Ok(await _repos.GetByName(search));
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Movie_ValidateIdFilterAttribute))]
        public async Task<IActionResult> GetById(int? id)
        {
            return Ok(HttpContext.Items["movie"]);
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Movie_ValidateIdFilterAttribute))]
        public IActionResult GetByIdNoTracking(int? id)
        {
            return Ok(HttpContext.Items["movie"]);
        }

        [HttpPost]
        [TypeFilter(typeof(Movie_ValidateCreateFilterAttribute))]
        public async Task<IActionResult> Post(Movie movie)
        {
            _repos.Add(movie);
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        //[TypeFilter(typeof(Movie_ValidateIdFilterAttribute))]
        [TypeFilter(typeof(Movie_ValidateUpdateFilterAttribute))]
        public async Task<IActionResult> Put(int id, Movie movie)
        {
            var dbMovie = await _repos.GetByIdNoTracking(movie.Id);
            if (dbMovie is null)
            {
                return NotFound(new ErrorResponse());
            }
            _repos.Update(movie);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Movie_ValidateIdFilterAttribute))]
        public async Task<IActionResult> Delete(int? id)
        {
            var movieDelete = HttpContext.Items["movie"] as Movie;
            _repos.Delete(movieDelete);
            return Ok(movieDelete);
        }
    }
}
