using Microsoft.AspNetCore.Mvc;
using Movies.DataAccess.Models;
using Movies.Models;
using Movies.Business.Repos.Interfaces;
using Movies.API.Filters;

namespace Movies.API.Controllers
{
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
                return NotFound(new ErrorModel()
                {
                    ErrorMessage = "No Movies available",
                    StatusCode = StatusCodes.Status404NotFound
                });
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
                //return NotFound("No Movies available");
                return NotFound(new ErrorModel()
                {
                    ErrorMessage = "No Movies available",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var movie = await _repos.GetById(id);
            if (movie == null)
            {
                //    return NotFound("Movie doesn't exist!");
                return NotFound(new ErrorModel()
                {
                    ErrorMessage = "Movie Not Found!",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(movie.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdNoTracking(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var movie = await _repos.GetByIdNoTracking(id);
            if (movie == null)
            {
                //return NotFound("Movie doesn't exist!");
                return NotFound(new ErrorModel()
                {
                    ErrorMessage = "Movie not Found!",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(movie.ToDto());
        }

        [HttpGet("{search}")]
        public async Task<IActionResult> GetByName(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid name",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var movies = await _repos.GetByName(search);
            if (movies == null)
            {
                //return NotFound("There are no Movies within that search!");
                return NotFound(new ErrorModel()
                {
                    ErrorMessage = "There are no Movies with that name!",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Movie movie)
        {
            if (movie is null)
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid Movie!",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var newMovie = _repos.Add(movie);
            if (newMovie == false)
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Could not create Movie",
                    StatusCode = StatusCodes.Status400BadRequest
                });
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
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Id mismatch!",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var dbMovie = await _repos.GetByIdNoTracking(movie.Id);
            if (dbMovie is null)
            {
                return NotFound(new ErrorModel()
                {
                    ErrorMessage = "Movie doesn't exist!",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            _repos.Update(movie);
            return Ok(movie.ToDto());
        }

        // DELETE: MoviesController/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                //return NotFound("Movie doesn't exist!");
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = "Invalid Movie!",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            var movie = await _repos.GetById(id);
            if (movie is null)
            {
                return NotFound(new ErrorModel()
                {
                    ErrorMessage = "Movie doesn't exist!",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            //var deleted = _repos.Delete(movie);
            return Ok(_repos.Delete(movie));
        }
    }
}
