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
            // Ensure HttpContext and User are not null before accessing them
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User == null)
            {
                return BadRequest("Unable to retrieve user information.");
            }

            var curUserId = httpContext.User.GetUserId();
            if (curUserId == null)
            {
                return BadRequest("User ID is null.");
            }

            var user = await _usersRepos.GetUserById(curUserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

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
