using Microsoft.AspNetCore.Mvc;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace Movies.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MoviesController(IMovieRepos _repos) : ControllerBase
    {
        // GET: MoviesController
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _repos.GetAll();
            if (movies == null || !movies.Any())
            {
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

        // GET: MoviesController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _repos.GetById(id);
            if (movie == null)
            {
                return NotFound("Movie doesn't exist!");
            }
            return Ok(movie.ToDto());
        }

        // GET: MoviesController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdNoTracking(int id)
        {
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
            var movie = await _repos.GetByName(search);
            if (movie == null)
            {
                return NotFound("There are no Movies within that search!");
            }
            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Movie movie)
        {
            var newMovie = _repos.Add(movie);
            if (newMovie == false)
            {
                return BadRequest("Could not create Movie");
            }
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        }

        // PUT: MoviesController/Edit/5
        [HttpPut]
        public async Task<IActionResult> Put(Movie movie)
        {
            if (movie is null)
            {
                return NotFound("Movie doesn't exist!");
            }
            
            var dbMovie = await _repos.GetByIdNoTracking(movie.Id);
            if (dbMovie is null)
            {
                return NotFound("Movie doesn't exist!");
            }

            _repos.Update(movie);
            return Ok(movie.ToDto());
        }

        // DELETE: MoviesController/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _repos.GetById(id);
            if (movie is null)
            {
                return NotFound("Movie doesn't exist!");
            }
            var deleted = _repos.Delete(movie);
            return Ok(deleted);
        }
    }
}
