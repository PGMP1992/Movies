using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Filters;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Models;
using Movies.Models;

namespace Movies.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName ="v2")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [JwtTokenAuth] // Custom filter for JWT authentication

    public class MoviesController(IMovieRepos _repos) : ControllerBase
    {
        // GET: MoviesController
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _repos.GetAll();

            if (movies == null || !movies.Any())
            {
                //return NotFound(new ErrorModel()
                //{
                //    ErrorMessage = "No Movies available",
                //    StatusCode = StatusCodes.Status404NotFound
                //});
                Console.WriteLine("Version 2.0 has been called");
                return NotFound("No Movies available");
            }

            return Ok(movies);
        }

        // GET: MoviesController
        [HttpGet]
        public async Task<IActionResult> GetAllActive()
        {
            var movies = await _repos.GetAllActive();
            if (movies == null || !movies.Any())
            {
                return NotFound("No Movies available");
            }
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest("Invalid Id");
            }

            var movie = await _repos.GetById(id);
            if (movie == null)
            {
                return NotFound("Movie doesn't exist!");
            }

            return Ok(movie.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdNoTracking(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest("Invalid Id");
            }

            var movie = await _repos.GetByIdNoTracking(id);
            if (movie == null)
            {
                return NotFound("Movie doesn't exist!");
            }
            return Ok(movie.ToDto());
        }

        [HttpGet("{search}")]
        public async Task<IActionResult> GetByName(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest("Invalid name");
            }

            var movies = await _repos.GetByName(search);
            if (movies == null)
            {
                return NotFound("There are no Movies within that search!");
            }
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Movie movie)
        {
            if (movie is null)
            {
                return BadRequest("Invalid Movie!");
            }

            var newMovie = _repos.Add(movie);
            if (newMovie == false)
            {
                return BadRequest("Could not create Movie");
            } 
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        }

        // PUT: MoviesController/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Movie movie)
        {
            if( id != movie.Id 
                || id <= 0
                || movie is null)
            {
                return BadRequest("Id mismatch!");
            }

            var dbMovie = await _repos.GetByIdNoTracking(movie.Id);
            if (dbMovie is null)
                return NotFound("Movie doesn't exist!");

            _repos.Update(movie);
            return Ok(movie.ToDto());
        }

        // DELETE: MoviesController/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest("Invalid Movie!");
            }
            var movie = await _repos.GetById(id);
            if (movie is null)
            {
                return NotFound("Movie doesn't exist!");
            }
            //var deleted = _repos.Delete(movie);
            return Ok(_repos.Delete(movie));
        }
    }
}
