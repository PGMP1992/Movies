using Microsoft.AspNetCore.Mvc;
using MoviesApp.Data;
using MoviesApp.Interfaces;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepos _dashboardRepos;

        public DashboardController(IDashboardRepos dashboardRepos)
        {
            _dashboardRepos = dashboardRepos;
        }
        public async Task<IActionResult> Index()
        {
            var userPlaylists = await _dashboardRepos.GetAllUserPlaylists();
            var userMovies = await _dashboardRepos.GetAllUserMovies();
            var dashboardVM = new DashboardVM()
            {
                Movies = userMovies,
                Playlists = userPlaylists
            };
            
            return View(dashboardVM);
        }
    }
}
