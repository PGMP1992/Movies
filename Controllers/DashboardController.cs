using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Data;
using MoviesApp.Repos.Interfaces;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUsersRepos _usersRepos;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IUsersRepos usersRepos,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _usersRepos = usersRepos;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _usersRepos.GetUserById(curUserId);

            var userPlaylists = await _usersRepos.GetAllUserPlaylists();

            var userDetailsVM = new UsersDetailsVM()
            {
                Playlists = userPlaylists,
                Id = curUserId,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageryUrl
            };

            return View(userDetailsVM);
        }
    }
}
