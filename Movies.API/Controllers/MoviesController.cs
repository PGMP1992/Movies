using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Filters;
using Movies.API.Filters.Movie;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Models;
using MoviesAPI.Filters.Movie;

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
            var movies = await _repos.GetAll();

            if (movies == null || !movies.Any())
            {
                return NotFound(new ErrorResponse());
            }
            return Ok(movies);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActive()
        {
            var movies = await _repos.GetAllActive();
            if (movies == null || !movies.Any())
            {
                return NotFound(new ErrorResponse());
            }
            return Ok(movies);
        }

        [TypeFilter(typeof(ValidateIdFilterAttribute))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            return Ok(HttpContext.Items["movie"]);
        }

        [TypeFilter(typeof(ValidateIdFilterAttribute))]
        [HttpGet("{id}")]
        public IActionResult GetByIdNoTracking(int? id)
        {
            return Ok(HttpContext.Items["movie"]);
        }

        [HttpGet("{search}")]
        public async Task<IActionResult> GetByName(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest(new ErrorResponse());
            }

            var movies = await _repos.GetByName(search);
            if (movies == null)
            {
                return NotFound(new ErrorResponse());
            }
            return Ok(movies);
        }

        
        [HttpPost]
        public async Task<IActionResult> Post(Movie movie)
        {
            if (movie is null)
            {
                return BadRequest(new ErrorResponse());
            }

            var newMovie = _repos.Add(movie);
            if (newMovie == false)
            {
                return BadRequest(new ErrorResponse());
            }
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        }

        [TypeFilter(typeof(ValidateUpdateFilterAttribute))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Movie movie)
        {
            //if( id != movie.Id 
            //    || id <= 0
            //    || movie is null)
            //{
            //    return BadRequest(new ErrorModelDto()
            //    {
            //        ErrorMessage = "Id mismatch!",
            //        StatusCode = StatusCodes.Status400BadRequest
            //    });
            //}

            var dbMovie = await _repos.GetByIdNoTracking(movie.Id);
            if (dbMovie is null)
            {
                return NotFound(new ErrorResponse());
            }
            _repos.Update(movie);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(ValidateIdFilterAttribute))]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null || id <= 0)
            //{
            //    return BadRequest(new ErrorModelDto()
            //    {
            //        ErrorMessage = "Invalid Movie!",
            //        StatusCode = StatusCodes.Status400BadRequest
            //    });
            //}
            
            var movie = await _repos.GetById(id);
            if (movie is null)
            {
                return NotFound(new ErrorResponse()); 
            }
            return Ok(_repos.Delete(movie));
        }
    }
}
