using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesApp.Interfaces;
using MoviesApp.Models;
using MoviesApp.ViewModels;
using MoviesApp.Repos;

namespace MoviesApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepos _movieRepos;
        private readonly IPlaylistRepos _playlistRepos;
        private readonly IPhotoService _photoService; //Cloudnary 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoviesController(IMovieRepos movieRepos, IPlaylistRepos playlistRepos,
                IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _movieRepos = movieRepos;
            _playlistRepos = playlistRepos;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string search)
        {
            var movies = await _movieRepos.GetAll();
            ViewBag.Message = "";

            if (!String.IsNullOrEmpty(search))
            {
                var movies1 = await _movieRepos.GetByName(search);
                if (movies1.Count > 0)
                {
                    return View(movies1);
                }
                ViewBag.Message = "There are not movies with that search.";
            }
            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepos.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["playlistName"] = new SelectList(await _playlistRepos.GetAll().ConfigureAwait(false), "Id", "Name");
            return View(movie);
        }

        // GET: Movies/Create
        //[Authorize]
        //[Authorize(Roles ="admin")]
        public IActionResult Create()
        {
            //var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var movieVM = new CreateMovieVM();// { AppUserId = curUserId }; 
            return View(movieVM);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Description,Genre,Age,PictUrl,BuyPrice,RentPrice")] Movie movie)
        public async Task<IActionResult> Create(CreateMovieVM movieVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(movieVM.PictUrl);
                var movie = new Movie
                {
                    //AppUserId = movieVM.AppUserId,
                    Title = movieVM.Title,
                    Description = movieVM.Description,
                    Genre = movieVM.Genre,
                    Age = movieVM.Age,
                    PictUrl = result.Url.ToString(),
                    //BuyPrice = movieVM.BuyPrice,
                    //RentPrice = movieVM.RentPrice
                };

                _movieRepos.Add(movie);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed. ");
            }
            return View(movieVM);
        }

        //[Authorize]
        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepos.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var movieVM = new EditMovieVM()
            {
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                Age = movie.Age,
                PictUrl = movie.PictUrl
            };
            return View(movieVM);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Genre,Age,PictUrl,BuyPrice,RentPrice")] Movie movie)
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Genre,Age,PictUrl")] Movie movie)
        public async Task<IActionResult> Edit(int id, EditMovieVM movieVM)
        {
            if (id != movieVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit Movie");
                return View("Edit", movieVM);
            }

            var photoResult = await _photoService.AddPhotoAsync(movieVM.Image);
            if (photoResult.Error != null)
            {
                ModelState.AddModelError("PictUrl", "Photo upload failed");
                return View(movieVM);
            }

            var movie = new Movie
            {
                Id = movieVM.Id,
                Title = movieVM.Title,
                Description = movieVM.Description,
                Genre = movieVM.Genre,
                Age = movieVM.Age,
                PictUrl = photoResult.Url.ToString()
            };

            _movieRepos.Update(movie);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Delete/5
        //[Authorize]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepos.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _movieRepos.GetByIdAsync(id);
            if (movie != null)
            {
                _movieRepos.Delete(movie);
            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
