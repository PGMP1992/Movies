using Microsoft.AspNetCore.Mvc;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;

namespace Movies.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepos _movieRepos;

        public MoviesController(IMovieRepos movieRepos)
        {
            _movieRepos = movieRepos;
        }

        // GET: MoviesController
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var movies = await _movieRepos.GetAll();
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
            var movies = await _movieRepos.GetAllActive();
            if (movies == null || !movies.Any())
            {
                return NotFound("No Movies available");
            }
            return Ok(movies);
        }

        // GET: MoviesController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var movie = await _movieRepos.GetById(id);
            if (movie == null)
            {
                return NotFound("Movie doesn't exist!");
            }
            return Ok(movie);
        }

        // GET: MoviesController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdNoTracking(int id)
        {
            var movie = await _movieRepos.GetByIdNoTracking(id);
            if (movie == null)
            {
                return NotFound("Movie doesn't exist!");
            }
            return Ok(movie);
        }

        [HttpGet("{search}")]
        public async Task<IActionResult> GetByName(string search)
        {
            var movie = await _movieRepos.GetByName(search);
            if (movie == null)
            {
                return NotFound("There are no Movies within that search!");
            }
            return Ok(movie);
        }

        // POST: MoviesController/Create
        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            var newMovie = _movieRepos.Add(movie);
            if (newMovie == false)
            {
                return BadRequest("Could not create Movie");
            }
            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }

        // PUT: MoviesController/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound("Movie doesn't exist!");
            }
            _movieRepos.Update(movie);
            return NoContent();
        }

        // DELETE: MoviesController/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepos.GetById(id);
            if (movie is null)
            {
                return NotFound("Movie doesn't exist!");
            }
            var deleted = _movieRepos.Delete(movie);
            return Ok(deleted);
        }
    }

}
